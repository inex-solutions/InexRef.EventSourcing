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

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Persistence.Common;
using InexRef.EventSourcing.Persistence.SqlServer.Persistence;
using InexRef.EventSourcing.Persistence.SqlServer.Utils;

namespace InexRef.EventSourcing.Persistence.SqlServer.NaturalKey
{
    public class SqlServerNaturalKeyToAggregateIdMap<TNaturalKey, TInternalId, TAggregate> : INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId, TAggregate>
    {
        private readonly SqlEventStoreConfiguration _sqlEventStoreConfiguration;
        private readonly IAggregateIdCreator<TInternalId> _aggregateIdCreator;
        private readonly ISqlParameterCreator<TNaturalKey> _naturalKeyParameterCreator;
        private readonly ISqlParameterCreator<TInternalId> _internalKeyParameterCreator;

        public SqlServerNaturalKeyToAggregateIdMap(
            SqlEventStoreConfiguration sqlEventStoreConfiguration,
            IAggregateIdCreator<TInternalId> aggregateIdCreator,
            ISqlParameterCreator<TNaturalKey> naturalKeyParameterCreator,
            ISqlParameterCreator<TInternalId> internalKeyParameterCreator)
        {
            _sqlEventStoreConfiguration = sqlEventStoreConfiguration;
            _aggregateIdCreator = aggregateIdCreator;
            _naturalKeyParameterCreator = naturalKeyParameterCreator;
            _internalKeyParameterCreator = internalKeyParameterCreator;
        }

        public Task<TInternalId> this[TNaturalKey naturalKey] => Get(naturalKey);

        private async Task<TInternalId> Get(TNaturalKey naturalKey)
        {
            using (var connection = new SqlConnection(_sqlEventStoreConfiguration.DbConnectionString))
            {
                var sql =
                    @"SELECT [AggregateId] FROM [NaturalKeyToAggregateIdMap-{aggName}] WHERE [NaturalKey] = @naturalKey"
                        .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName<TAggregate>());

                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(_naturalKeyParameterCreator.Create("@naturalKey", naturalKey));

                connection.Open();

                var result = await command.ExecuteScalarAsync();

                if (result == null)
                {
                    throw new KeyNotFoundException($"No entry was found for natural key {naturalKey}");
                }

                return (TInternalId) result;
            }
        }

        public async Task<TInternalId> CreateNew(TNaturalKey naturalKey)
        {
            using (var connection = new SqlConnection(_sqlEventStoreConfiguration.DbConnectionString))
            {
                connection.Open();
                var sql = @"
INSERT INTO [dbo].[NaturalKeyToAggregateIdMap-{aggName}] (NaturalKey, AggregateId)
VALUES (@naturalKey, @aggregateID)"
                    .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName<TAggregate>());

                var id = _aggregateIdCreator.Create();

                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(_naturalKeyParameterCreator.Create("@naturalKey", naturalKey));
                command.Parameters.Add(_internalKeyParameterCreator.Create("@aggregateID", id));

                await command.ExecuteNonQueryAsync();

                return id;
            }
        }

        public async Task<TInternalId> GetOrCreateNew(TNaturalKey naturalKey)
        {
            using (var connection = new SqlConnection(_sqlEventStoreConfiguration.DbConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    var sql = @"SELECT [AggregateId] FROM [dbo].[NaturalKeyToAggregateIdMap-{aggName}] WHERE [NaturalKey] = @naturalKey"
                        .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName<TAggregate>());

                    var command = new SqlCommand(sql, connection, transaction);
                    command.Parameters.Add(_naturalKeyParameterCreator.Create("@naturalKey", naturalKey));

                    object result = await command.ExecuteScalarAsync();

                    TInternalId id;

                    if (result != null)
                    {
                        id = (TInternalId) result;
                    }
                    else
                    {
                        sql = @"
INSERT INTO [dbo].[NaturalKeyToAggregateIdMap-{aggName}] (NaturalKey, AggregateId)
VALUES (@naturalKey, @aggregateID)"
                            .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName<TAggregate>());

                        id = _aggregateIdCreator.Create();

                        command = new SqlCommand(sql, connection, transaction);
                        command.Parameters.Add(_naturalKeyParameterCreator.Create("@naturalKey", naturalKey));
                        command.Parameters.Add(_internalKeyParameterCreator.Create("@aggregateID", id));

                        await command.ExecuteNonQueryAsync();
                    }

                    transaction.Commit();

                    return id;
                }
            }
        }

        public async Task Delete(TNaturalKey naturalKey)
        {
            using (var connection = new SqlConnection(_sqlEventStoreConfiguration.DbConnectionString))
            {
                var sql = @"DELETE FROM [NaturalKeyToAggregateIdMap-{aggName}] WHERE [NaturalKey] = @naturalKey"
                    .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName<TAggregate>());

                var command = new SqlCommand(sql, connection);
                command.Parameters.Add(_naturalKeyParameterCreator.Create("@naturalKey", naturalKey));

                connection.Open();

                await command.ExecuteNonQueryAsync();
            }
        }

        public IEnumerable<TNaturalKey> GetAllKeys()
        {
            using (var connection = new SqlConnection(_sqlEventStoreConfiguration.DbConnectionString))
            {
                var sql = @"SELECT [NaturalKey] FROM [NaturalKeyToAggregateIdMap-{aggName}]"
                    .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName<TAggregate>());

                var command = new SqlCommand(sql, connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return (TNaturalKey) reader[0];
                    }
                }
            }
        }
    }
}