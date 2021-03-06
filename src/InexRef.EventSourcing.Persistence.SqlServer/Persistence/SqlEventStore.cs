#region Copyright & Licence
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Persistence.Common;
using InexRef.EventSourcing.Persistence.SqlServer.Utils;

namespace InexRef.EventSourcing.Persistence.SqlServer.Persistence
{
    public class SqlEventStore<TId> : IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly SqlEventStoreConfiguration _sqlEventStoreConfiguration;
        private readonly ISqlEventStoreJsonSerializer _serializer;
        private readonly ISqlParameterCreator<TId> _idParameterCreator;

        public SqlEventStore(SqlEventStoreConfiguration sqlEventStoreConfiguration, 
            ISqlEventStoreJsonSerializer serializer, 
            ISqlParameterCreator<TId> idParameterCreator)
        {
            _sqlEventStoreConfiguration = sqlEventStoreConfiguration;
            _serializer = serializer;
            _idParameterCreator = idParameterCreator;
        }

        public IEnumerable<IEvent<TId>> LoadEvents(TId aggregateId, Type aggregateType, bool throwIfNotFound)
        {
            using (var connection = new SqlConnection(_sqlEventStoreConfiguration.DbConnectionString))
            {
                connection.Open();

                var sql = @"  
SELECT [EventDateTime], [SourceCorrelationId], [Payload] FROM [dbo].[EventStore-{aggName}] WHERE [AggregateId] = @aggregateId ORDER BY [Version] ASC"
                    .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName(aggregateType));

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add(_idParameterCreator.Create("@aggregateID", aggregateId));

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            if (throwIfNotFound)
                            {
                                throw new AggregateNotFoundException($"Aggregate '{aggregateId}' not found");
                            }

                            yield break;
                        }

                        while (reader.Read())
                        {
                            var @event = (IEvent<TId>)_serializer.Deserialize((string)reader[2], (string)reader[1], (DateTime)reader[0]);
                            yield return @event;
                        }
                    }
                }
            }
        }

        public async Task SaveEvents(TId id, Type aggregateType, IEnumerable<IEvent<TId>> events, int currentVersion, int expectedVersion)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("AggregateId", typeof(TId));
            dataTable.Columns.Add("Version", typeof(long));
            dataTable.Columns.Add("EventDateTime", typeof(DateTime));
            dataTable.Columns.Add("SourceCorrelationId", typeof(string));
            dataTable.Columns.Add("Payload", typeof(string));

            foreach (var @event in events)
            {
                dataTable.Rows.Add(
                    @event.Id, 
                    @event.Version, 
                    @event.MessageMetadata.MessageDateTime, 
                    @event.MessageMetadata.SourceCorrelationId,
                    _serializer.Serialize(@event));
            }

            using (var connection = new SqlConnection(_sqlEventStoreConfiguration.DbConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_Insert{aggName}Events".Replace("{aggName}", AggregateRootUtils.GetAggregateRootName(aggregateType));
                    command.Parameters.Add(_idParameterCreator.Create("@aggregateID", id));
                    command.Parameters.Add("@expectedVersion", SqlDbType.BigInt).Value = expectedVersion;

                    var tableParameter = new SqlParameter();
                    tableParameter.ParameterName = "@eventsToInsert";
                    tableParameter.TypeName = "EventStoreType";
                    tableParameter.SqlDbType = SqlDbType.Structured;
                    tableParameter.Value = dataTable;
                    command.Parameters.Add(tableParameter);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex) when (ex.Number == 51000)
                    {
                        throw new EventStoreConcurrencyException(ex.Message);
                    }
                }
            }
        }

        public async Task DeleteEvents(TId id, Type aggregateType)
        {
            using (var connection = new SqlConnection(_sqlEventStoreConfiguration.DbConnectionString))
            {
                await connection.OpenAsync();

                var getLatestVersionSql = @"DELETE FROM [dbo].[EventStore-{aggName}] WHERE [AggregateId] = @aggregateId".Replace("{aggName}", AggregateRootUtils.GetAggregateRootName(aggregateType));

                using (var command = new SqlCommand(getLatestVersionSql, connection))
                {
                    command.Parameters.Add(_idParameterCreator.Create("@aggregateID", id));
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

       
    }
}