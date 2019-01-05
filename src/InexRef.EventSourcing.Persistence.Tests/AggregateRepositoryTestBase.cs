using System;
using System.Collections.Generic;
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.Common;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace InexRef.EventSourcing.Persistence.Tests
{
    [TestFixtureSource(typeof(NUnitTestFixtureSource), "TestFlavours")]
    public abstract class AggregateRepositoryTestBase : SpecificationBaseAsync<IAggregateRepository<CounterAggregateRoot, Guid>>
    {
        private readonly string _testFlavour;
        private ServiceProvider _container;

        protected Guid AggregateId { get; private set; }

        protected CounterAggregateRoot ReloadedCounterAggregateRoot { get; set; }

        protected List<Guid> CreatedGuids { get; } = new List<Guid>();

        protected IAggregateRootFactory AggregateRootFactory { get; private set; }

        protected AggregateRepositoryTestBase(string testFlavour)
        {
            _testFlavour = testFlavour;
        }

        protected override void SetUp()
        {
            var containerBuilder = new ServiceCollection();
            TestEnvironmentSetup.ConfigureContainerForHostEnvironmentFlavour(containerBuilder, _testFlavour);
            containerBuilder.RegisterModule<EventSourcingCoreModule>();
            containerBuilder.AddTransient<CounterAggregateRoot>();
            containerBuilder.AddTransient<NonDisposingCounterAggregateRoot>();

            _container = containerBuilder.BuildServiceProvider();

            AggregateId = CreateAggregateId();
            AggregateRootFactory = _container.GetRequiredService<IAggregateRootFactory>();
            Subject = _container.GetRequiredService<IAggregateRepository<CounterAggregateRoot, Guid>>();
        }

        private Guid CreateAggregateId()
        {
            var guid = Guid.NewGuid();
            CreatedGuids.Add(guid);
            return guid;
        }

        protected override void Cleanup()
        {
            _container.Dispose();
            Subject.Delete(AggregateId);
            ReloadedCounterAggregateRoot?.Dispose();
        }

        public void DirectlyReferenceNUnitToAidTestRunner()
        {
            Assert.IsTrue(true);
        }
    }
}