using System;
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace InexRef.EventSourcing.Tests.Common
{
    [TestFixtureSource(typeof(NUnitTestFixtureSource), "TestFlavours")]
    public abstract class IntegrationTestBase <TAggregate, TNaturalKey>: SpecificationBaseAsync<IBus> 
        where TAggregate : class, IAggregateRoot<Guid>, IAggregateRootInternal<Guid>
        where TNaturalKey : IEquatable<TNaturalKey>, IComparable<TNaturalKey>
    {
        private ServiceProvider _container;

        private string _flavour;

        protected TNaturalKey NaturalId { get; set; }

        protected INaturalKeyDrivenAggregateRepository<TAggregate, Guid, TNaturalKey> Repository { get; private set; }

        protected IntegrationTestBase(string flavour)
        {
            _flavour = flavour;
        }

        protected override void SetUp()
        {
            var serviceCollection = new ServiceCollection();
            TestEnvironmentSetup.ConfigureContainerForHostEnvironmentFlavour(serviceCollection, _flavour);

            serviceCollection.RegisterModule<EventSourcingCoreModule>();
            serviceCollection.AddTransient<TAggregate>();

            RegisterWithContainerBuilder(serviceCollection);

            _container = serviceCollection.BuildServiceProvider();

            ResolveFromContainer(_container);

            Subject = _container.GetRequiredService<IBus>();
            Repository = _container.GetRequiredService<INaturalKeyDrivenAggregateRepository<TAggregate, Guid, TNaturalKey>>();
        }

        protected virtual void RegisterWithContainerBuilder(IServiceCollection containerBuilder)
        {
        }

        protected virtual void ResolveFromContainer(IServiceProvider container)
        {
        }
        protected override void Cleanup()
        {
            Repository.DeleteByNaturalKey(NaturalId);
            _container.Dispose();
        }
    }
}
