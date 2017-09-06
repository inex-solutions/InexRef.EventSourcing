
using System;
using Autofac;
using NUnit.Framework;
using Rob.EventSourcing.Persistence;
using Rob.EventSourcing.Tests.SpecificationTests;

namespace Rob.EventSourcing.Tests.PersistenceTests
{
    [TestFixture("FileSystem")]
    [TestFixture("InMemory")]
    public abstract class AggregateRepositoryTestBase : SpecificationBase<IAggregateRepository<AccountAggregateRoot>>
    {
        private readonly string _persistenceProvider;

        protected Guid AggregateId { get; private set; }

        protected AccountAggregateRoot ReloadedAccountAggregateRoot { get; set; }

        protected AggregateRepositoryTestBase(string persistenceProvider)
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

            Subject = container.Resolve<IAggregateRepository<AccountAggregateRoot>>();

            AggregateId = Guid.NewGuid();

        }

        protected override void Cleanup()
        {
            Subject.Delete(AggregateId);
        }
    }
}