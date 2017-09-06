
using System;
using Autofac;
using Rob.EventSourcing.Persistence;
using Rob.EventSourcing.Tests.SpecificationTests;

namespace Rob.EventSourcing.Tests.PersistenceTests
{
    public abstract class AggregateRepositoryTestBase : SpecificationBase<IAggregateRepository<AccountAggregateRoot>>
    {
        protected Guid AggregateId { get; private set; }

        protected AccountAggregateRoot ReloadedAccountAggregateRoot { get; set; }

        protected override void SetUp()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<EventSourcingCoreModule>();
            containerBuilder.RegisterModule<EventSourcingInMemoryInfrastructureModule>();

            // experimenting with file system persistence
            var filePersistenceConfiguration = new FilePersistenceConfiguration
            {
                EventStoreRootDirectory = "d:\\temp\\EventStorePersistence"
            };
            containerBuilder.RegisterInstance(filePersistenceConfiguration);
            containerBuilder.RegisterType<FileSystemEventStore>().As<IEventStore>().SingleInstance();

            var container = containerBuilder.Build();

            Subject = container.Resolve<IAggregateRepository<AccountAggregateRoot>>();

            AggregateId = Guid.NewGuid();

        }

        protected override void Cleanup()
        {
            Subject.Delete(AggregateId);
        }
    }
}