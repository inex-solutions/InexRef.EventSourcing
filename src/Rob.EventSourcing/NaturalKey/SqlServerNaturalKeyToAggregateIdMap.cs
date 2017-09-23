using System;
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