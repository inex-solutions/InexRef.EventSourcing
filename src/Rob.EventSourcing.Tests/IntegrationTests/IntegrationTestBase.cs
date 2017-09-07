using System;
using Autofac;
using NUnit.Framework;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Persistence;
using Rob.EventSourcing.Tests.SpecificationTests;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    [TestFixture("FileSystem")]
    [TestFixture("InMemory")]
    public abstract class IntegrationTestBase : SpecificationBase<IBus>
    {
        private readonly string _persistenceProvider;

        protected string AggregateId { get; private set; }

        protected IAggregateRepository<AccountAggregateRoot, string> Repository { get; private set; }

        protected BalanceReadModel BalanceReadModel { get; private set; }

        protected ReceivedEventsHistoryReadModel ReceivedEventsHistoryReadModel { get; private set; }

        protected IntegrationTestBase(string persistenceProvider)
        {
            _persistenceProvider = persistenceProvider;
        }

        protected override void SetUp()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<EventSourcingCoreModule>();

            switch (_persistenceProvider)
            {
                case "InMemory":
                    containerBuilder.RegisterModule<EventSourcingInMemoryInfrastructureModule>();
                    break;
                case "FileSystem":
                    containerBuilder.RegisterModule<EventSourcingFileSystemInfrastructureModule>();
                    break;
                default:
                    throw new TestSetupException($"Test setup failed. Persistence provider '{_persistenceProvider}' not supported.");
            }

            var container = containerBuilder.Build();
            Subject = container.Resolve<IBus>();
            Repository = container.Resolve<IAggregateRepository<AccountAggregateRoot, string>>();

            AggregateId = Guid.NewGuid().ToString();

            BalanceReadModel = new BalanceReadModel();
            Subject.Subscribe<BalanceUpdatedEvent>(BalanceReadModel.Handle);
            Subject.Subscribe<BalanceResetEvent>(BalanceReadModel.Handle);

            ReceivedEventsHistoryReadModel = new ReceivedEventsHistoryReadModel();
            Subject.Subscribe<BalanceUpdatedEvent>(ReceivedEventsHistoryReadModel.Handle);
            Subject.Subscribe<BalanceResetEvent>(ReceivedEventsHistoryReadModel.Handle);

            var handlers = new IntegrationTestHandlers(Repository);
            Subject.RegisterHandler<AddAmountCommand>(handlers.Handle);
            Subject.RegisterHandler<ResetBalanceCommand>(handlers.Handle);
        }

        protected override void Cleanup()
        {
            Repository.Delete(AggregateId);
        }
    }
}
