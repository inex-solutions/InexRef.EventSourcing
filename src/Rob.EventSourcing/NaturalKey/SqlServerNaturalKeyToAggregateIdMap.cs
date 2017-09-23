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

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Rob.EventSourcing.Persistence;
using Rob.EventSourcing.Utils;

namespace Rob.EventSourcing.NaturalKey
{
    public class SqlServerNaturalKeyToAggregateIdMap<TNaturalKey, TInternalId, TAggregate> : INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId, TAggregate>
    {
        private readonly SqlServerPersistenceConfiguration _sqlServerPersistenceConfiguration;
        private readonly IAggregateIdCreator<TInternalId> _aggregateIdCreator;

        public SqlServerNaturalKeyToAggregateIdMap(
            SqlServerPersistenceConfiguration sqlServerPersistenceConfiguration,
            IAggregateIdCreator<TInternalId> aggregateIdCreator)
        {
            _sqlServerPersistenceConfiguration = sqlServerPersistenceConfiguration;
            _aggregateIdCreator = aggregateIdCreator;
        }

        public TInternalId this[TNaturalKey naturalKey]
        {
            get
            {
                using (var connection = new SqlConnection(_sqlServerPersistenceConfiguration.ConnectionString))
                {
                    var sql = @"SELECT [AggregateId] FROM [NaturalKeyToAggregateIdMap-{aggName}] WHERE [NaturalKey] = @naturalKey"
                        .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName< TAggregate>());

                    var command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@naturalKey", SqlDbTypeUtils.GetSqlDbType<TNaturalKey>()).Value = naturalKey;

                    connection.Open();

                    var result = command.ExecuteScalar();

                    if (result == null)
                    {
                        throw new KeyNotFoundException($"No entry was found for natural key {naturalKey}");
                    }

                    return (TInternalId) result;
                }
            }
        }

        public TInternalId GetOrCreateNew(TNaturalKey naturalKey)
        {
            using (var connection = new SqlConnection(_sqlServerPersistenceConfiguration.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    var sql = @"SELECT [AggregateId] FROM [dbo].[NaturalKeyToAggregateIdMap-{aggName}] WHERE [NaturalKey] = @naturalKey"
                        .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName<TAggregate>());

                    var command = new SqlCommand(sql, connection, transaction);
                    command.Parameters.Add("@naturalKey", SqlDbTypeUtils.GetSqlDbType<TNaturalKey>()).Value = naturalKey;

                    object result = command.ExecuteScalar();

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
                        command.Parameters.Add("@naturalKey", SqlDbTypeUtils.GetSqlDbType<TNaturalKey>()).Value = naturalKey;
                        command.Parameters.Add("@aggregateID", SqlDbTypeUtils.GetSqlDbType<TInternalId>()).Value = id;

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    return id;
                }
            }
        }

        public void Delete(TNaturalKey naturalKey)
        {
            using (var connection = new SqlConnection(_sqlServerPersistenceConfiguration.ConnectionString))
            {
                var sql = @"DELETE FROM [NaturalKeyToAggregateIdMap-{aggName}] WHERE [NaturalKey] = @naturalKey"
                    .Replace("{aggName}", AggregateRootUtils.GetAggregateRootName<TAggregate>());

                var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@naturalKey", SqlDbTypeUtils.GetSqlDbType<TNaturalKey>()).Value = naturalKey;

                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}