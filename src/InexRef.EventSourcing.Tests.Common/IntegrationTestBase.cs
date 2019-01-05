#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

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
