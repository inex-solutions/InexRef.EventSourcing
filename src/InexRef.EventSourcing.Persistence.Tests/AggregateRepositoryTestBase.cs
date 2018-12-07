#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2018 INEX Solutions Ltd
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
using System.Collections.Generic;
using System.Linq;
using Autofac;
using InexRef.EventSourcing.Common;
using InexRef.EventSourcing.Common.Scoping;
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.Common;
using InexRef.EventSourcing.Tests.Common.Persistence;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using NUnit.Framework;

namespace InexRef.EventSourcing.Persistence.Tests
{
    [TestFixture("EventStorePersistence=InMemory", Category = "DomainOnly")]
    [TestFixture("EventStorePersistence=SqlServer", Category = "DomainHosting")]
    [TestFixture("EventStorePersistence=EventStore", Category = "DomainHosting")]
    public abstract class AggregateRepositoryTestBase : SpecificationBaseAsync<IAggregateRepository<CounterAggregateRoot, Guid>>
    {
        private readonly IDictionary<string, string> _testFixtureOptions;
        private IContainer _container;

        protected Guid AggregateId { get; private set; }

        protected CounterAggregateRoot ReloadedCounterAggregateRoot { get; set; }

        protected List<Guid> CreatedGuids { get; } = new List<Guid>();

        protected IAggregateRootFactory AggregateRootFactory { get; private set; }

        protected AggregateRepositoryTestBase(string testFixtureOptions)
        {
            _testFixtureOptions = testFixtureOptions
                .Split(',')
                .ToDictionary(item => item.Split('=')[0].Trim(), item => item.Split('=')[1].Trim());
        }

        protected void CreateNewScope()
        {
            OperationScope?.Dispose();

            OperationScope = _container
                .Resolve<IOperationScopeManager>()
                .CreateScope(Guid.NewGuid().ToString(), _container.Resolve<IDateTimeProvider>().GetUtcNow());

            Subject = OperationScope.Get<IAggregateRepository<CounterAggregateRoot, Guid>>();
            AggregateRootFactory = OperationScope.Get<IAggregateRootFactory>();
        }

        protected override void SetUp()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<EventSourcingCoreModule>();
            containerBuilder.RegisterEventStorePersistenceModule(_testFixtureOptions["EventStorePersistence"]);
            containerBuilder.RegisterModule<TestSetupModule>();
            containerBuilder.RegisterType<CounterAggregateRoot>();
            containerBuilder.RegisterType<NonDisposingCounterAggregateRoot>();

            _container = containerBuilder.Build();

            CreateNewScope();
            AggregateId = CreateAggregateId();
        }

        protected IOperationScope OperationScope { get; set; }

        protected Guid CreateAggregateId()
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