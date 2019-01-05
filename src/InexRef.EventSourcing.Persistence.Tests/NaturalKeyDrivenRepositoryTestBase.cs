using System;
using System.Collections.Generic;
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.Common;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace InexRef.EventSourcing.Persistence.Tests
{
    [TestFixtureSource(typeof(NUnitTestFixtureSource), "TestFlavours")]
    public abstract class NaturalKeyDrivenRepositoryTestBase : SpecificationBase<INaturalKeyDrivenAggregateRepository<CounterAggregateRoot, Guid, string>>
    {
        private readonly string _flavour;

        private ServiceProvider _container;

        protected List<string> CreatedIds { get; } = new List<string>();

        protected NaturalKeyDrivenRepositoryTestBase(string flavour)
        {
            _flavour = flavour;
        }

        protected override void SetUp()
        {
            var containerBuilder = new ServiceCollection();
            TestEnvironmentSetup.ConfigureContainerForHostEnvironmentFlavour(containerBuilder, _flavour);
            containerBuilder.RegisterModule<EventSourcingCoreModule>();
            containerBuilder.AddTransient<CounterAggregateRoot>();
            containerBuilder.AddTransient<NonDisposingCounterAggregateRoot>();

            _container = containerBuilder.BuildServiceProvider();

            Subject = _container.GetRequiredService<INaturalKeyDrivenAggregateRepository<CounterAggregateRoot, Guid, string>>();
        }

        protected override void Cleanup()
        {
            foreach (var id in CreatedIds)
            {
                Subject.DeleteByNaturalKey(id);
            }

            _container.Dispose();
        }

        public void DirectlyReferenceNUnitToAidTestRunner()
        {
            Assert.IsTrue(true);
        }
    }
}