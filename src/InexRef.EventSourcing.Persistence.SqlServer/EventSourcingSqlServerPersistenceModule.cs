using System;
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Common.Hosting;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Persistence.SqlServer.NaturalKey;
using InexRef.EventSourcing.Persistence.SqlServer.Persistence;
using InexRef.EventSourcing.Persistence.SqlServer.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Persistence.SqlServer
{
    public class EventSourcingSqlServerPersistenceModule : ContainerConfigurationModule
    {
        protected override void Load(IServiceCollection containerBuilder)
        {
            containerBuilder.AddTransient<ISqlEventStoreJsonSerializer, SqlEventStoreJsonSerializer>();
            containerBuilder.AddSingleton(typeof(IEventStore<>), typeof(SqlEventStore<>));
            containerBuilder.AddSingleton(typeof(INaturalKeyToAggregateIdMap<,,>), typeof(SqlServerNaturalKeyToAggregateIdMap<,,>));

            containerBuilder.AddTransient<ISqlParameterCreator<string>, StringSqlParameterCreator>();
            containerBuilder.AddTransient<ISqlParameterCreator<Guid>, GuidSqlParameterCreator>();

            var sqlConfig = new SqlEventStoreConfiguration();
            HostedEnvironmentConfiguration.ConfigurationRoot.GetSection("SqlEventStore").Bind(sqlConfig);
            containerBuilder.AddSingleton(sqlConfig);
        }
    }
}