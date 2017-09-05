
using System;
using Autofac;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Persistence;
using Rob.EventSourcing.Tests.SpecificationTests;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase : SpecificationBase<IBus>
    {
        protected DisposableDirectory DisposableDirectory;

        protected Guid AggregateId { get; private set; }

        protected IAggregateRepository<AccountAggregateRoot> Repository { get; private set; }

        protected BalanceReadModel BalanceReadModel { get; private set; }

        protected override void SetUp()
        {
            DisposableDirectory = new DisposableDirectory();

            var filePersistenceConfiguration = new FilePersistenceConfiguration
            {
                EventStoreRootDirectory = DisposableDirectory.FullName
            };

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<EventSourcingCoreModule>();
            containerBuilder.RegisterModule<EventSourcingInMemoryInfrastructureModule>();
            containerBuilder.RegisterInstance(filePersistenceConfiguration);
            containerBuilder.RegisterType<FileSystemEventStore>().As<IEventStore>().SingleInstance(); // experimenting with file system

            var container = containerBuilder.Build();
            Subject = container.Resolve<IBus>();
            Repository = container.Resolve<IAggregateRepository<AccountAggregateRoot>>();

            AggregateId = Guid.NewGuid();

            BalanceReadModel = new BalanceReadModel();
            Subject.Subscribe<BalanceUpdatedEvent>(BalanceReadModel.Handle);
            Subject.Subscribe<BalanceResetEvent>(BalanceReadModel.Handle);

            var handlers = new IntegrationTestHandlers(Repository);
            Subject.RegisterHandler<AddAmountCommand>(handlers.Handle);
            Subject.RegisterHandler<ResetBalanceCommand>(handlers.Handle);
        }

        protected override void Cleanup()
        {
            DisposableDirectory.Dispose();
        }
    }
}
