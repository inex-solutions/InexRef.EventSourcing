using Autofac;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Contracts.Bus;
using Rob.EventSourcing.Contracts.Persistence;
using Rob.EventSourcing.Persistence;

namespace Rob.EventSourcing
{
    public class EventSourcingSqlServerInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryBus>().As<IBus>().As<IEventBus>().As<ICommandBus>().SingleInstance();
            var sqlServerPersistenceConfiguration = new SqlServerPersistenceConfiguration
            {
                ConnectionString = "Server=localhost;Database=Rob.EventStore;Trusted_Connection=True"
            };
            builder.RegisterInstance(sqlServerPersistenceConfiguration);
            builder.RegisterGeneric(typeof(SqlEventStore<>)).As(typeof(IEventStore<>)).SingleInstance();
        }
    }
}