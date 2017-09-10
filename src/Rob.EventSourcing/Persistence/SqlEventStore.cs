#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017 INEX Solutions Ltd
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
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Rob.EventSourcing.Contracts.Messages;
using Rob.EventSourcing.Contracts.Persistence;

namespace Rob.EventSourcing.Persistence
{
    public class SqlEventStore<TId> : IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly SqlServerPersistenceConfiguration _sqlServerPersistenceConfiguration;
        private readonly JsonSerializer _jsonSerializer;

        public SqlEventStore(SqlServerPersistenceConfiguration sqlServerPersistenceConfiguration)
        {
            _sqlServerPersistenceConfiguration = sqlServerPersistenceConfiguration;
            _jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
        }

        public IEnumerable<IEvent<TId>> LoadEvents(TId aggregateId, bool throwIfNotFound)
        {
            using (var connection = new SqlConnection(_sqlServerPersistenceConfiguration.ConnectionString))
            {
                connection.Open();

                var sql = @"  
SELECT [Payload] FROM [dbo].[EventStore-{aggName}] WHERE [AggregateId] = @aggregateId ORDER BY [Version] ASC"
                    .Replace("{aggName}", "Account");

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@aggregateId", SqlDbType.NVarChar).Value = aggregateId;

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
                            yield return (IEvent<TId>) Deserialize((string) reader[0]);
                        }
                    }
                }
            }
        }

        public void SaveEvents(TId id, Type aggregateType, IEnumerable<IEvent<TId>> events, int currentVersion, int expectedVersion)
        {
            using (var connection = new SqlConnection(_sqlServerPersistenceConfiguration.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    const string insertSql = @"
WITH cte AS
(
SELECT @aggregateId AS [AggregateId], @version AS [Version], @eventDateTime AS [EventDateTime], @payload AS [Payload]
)

INSERT INTO [dbo].[EventStore-{aggName}] (AggregateId, Version, EventDateTime, Payload)  
SELECT 
	cte.AggregateId,
	cte.[Version],
	cte.[EventDateTime],
	cte.[Payload]
FROM cte
	LEFT JOIN [Rob.EventStore].[dbo].[EventStore-{aggName}] store ON cte.AggregateId = store.AggregateId 
WHERE 
	cte.AggregateId = @aggregateId
GROUP BY
	cte.AggregateId,
	cte.[Version],
	cte.[EventDateTime],
	cte.[Payload]
HAVING 
	MAX(store.[Version]) = @expectedVersion OR COUNT(store.[Version]) = 0
";

                    foreach (var @event in events)
                    {
                        var insertEventsSql = insertSql.Replace("{aggName}", "Account");

                        using (var command = new SqlCommand(insertEventsSql, connection, transaction))
                        {
                            command.Parameters.Add("@aggregateId", SqlDbType.NVarChar).Value = @event.Id;
                            command.Parameters.Add("@version", SqlDbType.BigInt).Value = @event.Version;
                            command.Parameters.Add("@eventDateTime", SqlDbType.DateTime2).Value = DateTime.Now; //TODO: remove this hack
                            command.Parameters.Add("@payload", SqlDbType.NVarChar).Value = Serialize(@event);
                            command.Parameters.Add("@expectedVersion", SqlDbType.BigInt).Value = expectedVersion;
                            var numRecordsUpdated = command.ExecuteNonQuery();

                            if (numRecordsUpdated == 0)
                            {
                                throw new EventStoreConcurrencyException($"Concurrency error saving aggregate {id} (expected version {expectedVersion})");
                            }

                        }
                    }

                    transaction.Commit();
                }
            }
        }

        public void DeleteEvents(TId id, Type aggregateType)
        {
            using (var connection = new SqlConnection(_sqlServerPersistenceConfiguration.ConnectionString))
            {
                connection.Open();

                var getLatestVersionSql = @"DELETE FROM [dbo].[EventStore-{aggName}] WHERE [AggregateId] = @aggregateId".Replace("{aggName}", "Account");

                using (var command = new SqlCommand(getLatestVersionSql, connection))
                {
                    command.Parameters.Add("@aggregateId", SqlDbType.NVarChar).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        private string Serialize(IEvent @event)
        {
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms, Encoding.UTF8))
            {
                _jsonSerializer.Serialize(writer, @event);
                writer.Flush();
                ms.Position = 0;

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private IEvent Deserialize(string eventPayload)
        {
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms, Encoding.UTF8))
            {
                writer.Write(eventPayload);
                writer.Flush();
                ms.Position = 0;

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return (IEvent)_jsonSerializer.Deserialize(reader, typeof(IEvent));
                }
            }
        }
    }
}