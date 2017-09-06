using Autofac;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Persistence;

namespace Rob.EventSourcing
{
    public class EventSourcingFileSystemInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryBus>().As<IBus>().As<IEventBus>().As<ICommandBus>().SingleInstance();
            var filePersistenceConfiguration = new FilePersistenceConfiguration
            {
                EventStoreRootDirectory = "d:\\temp\\EventStorePersistence"
            };
            builder.RegisterInstance(filePersistenceConfiguration);
            builder.RegisterType<FileSystemEventStore>().As<IEventStore>().SingleInstance();
        }
    }
}