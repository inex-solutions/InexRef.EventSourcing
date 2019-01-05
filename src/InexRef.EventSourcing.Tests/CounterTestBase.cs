using System;
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Tests.Common;
using InexRef.EventSourcing.Tests.Domain;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace InexRef.EventSourcing.Tests
{
    public abstract class CounterTestBase : IntegrationTestBase<CounterAggregateRoot, string>
    {
        protected CounterTestBase(string hostingFlavour) : base(hostingFlavour)
        {
            NaturalId = new IdGenerator("my-root").CreateAggregateId();
        }

        protected override void RegisterWithContainerBuilder(IServiceCollection containerBuilder)
        {
            containerBuilder.RegisterModule<CounterDomainHostModule>();
            containerBuilder.AddTransient<CounterAggregateRoot>();
        }

        protected override void ResolveFromContainer(IServiceProvider container)
        {
        }

        public void DirectlyReferenceNUnitToAidTestRunner()
        {
            Assert.IsTrue(true);
        }
    }
}