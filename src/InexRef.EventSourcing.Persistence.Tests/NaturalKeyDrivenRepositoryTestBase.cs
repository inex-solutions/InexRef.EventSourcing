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
    public abstract class NaturalKeyDrivenRepositoryTestBase : SpecificationBase<INaturalKeyDrivenAggregateRepository<CounterAggregateRoot, Guid, string>>
    {
        private readonly IDictionary<string, string> _testFixtureOptions;

        private IContainer _container;

        protected List<string> CreatedIds { get; } = new List<string>();

        protected NaturalKeyDrivenRepositoryTestBase(string testFixtureOptions)
        {
            _testFixtureOptions = testFixtureOptions
                .Split(',')
                .ToDictionary(item => item.Split('=')[0].Trim(), item => item.Split('=')[1].Trim());
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

            Subject = _container.Resolve<INaturalKeyDrivenAggregateRepository<CounterAggregateRoot, Guid, string>>();
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