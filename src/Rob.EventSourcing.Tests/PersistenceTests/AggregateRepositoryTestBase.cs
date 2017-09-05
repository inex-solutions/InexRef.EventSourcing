
using System;
using Autofac;
using Rob.EventSourcing.Persistence;
using Rob.EventSourcing.Tests.SpecificationTests;

namespace Rob.EventSourcing.Tests.PersistenceTests
{
    public abstract class AggregateRepositoryTestBase : SpecificationBase<IAggregateRepository<AccountAggregateRoot>>
    {
        protected DisposableDirectory DisposableDirectory;

        protected Guid AggregateId { get; private set; }

        protected AccountAggregateRoot ReloadedAccountAggregateRoot { get; set; }

        protected override void SetUp()
        {
            DisposableDirectory = new DisposableDirectory();

            var filePersistenceConfiguration = new FilePersistenceConfiguration
            {
                EventStoreRootDirectory = DisposableDirectory.FullName
            };

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(filePersistenceConfiguration);
            containerBuilder.RegisterModule<EventSourcingCoreModule>();
            containerBuilder.RegisterModule<EventSourcingInMemoryInfrastructureModule>();
            var container = containerBuilder.Build();

            Subject = container.Resolve<IAggregateRepository<AccountAggregateRoot>>();

            AggregateId = Guid.NewGuid();

        }

        protected override void Cleanup()
        {
            DisposableDirectory.Dispose();
        }
    }
}