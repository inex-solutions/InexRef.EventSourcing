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
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.SpecificationTests;
using NUnit.Framework;

namespace InexRef.EventSourcing.Tests.PersistenceTests
{
    [TestFixture("EventStorePersistence=InMemory")]
    [TestFixture("EventStorePersistence=SqlServer")]
    public abstract class AggregateRepositoryTestBase : SpecificationBase<IAggregateRepository<AccountAggregateRoot, Guid>>
    {
        private readonly IDictionary<string, string> _testFixtureOptions;

        protected Guid AggregateId { get; private set; }

        protected AccountAggregateRoot ReloadedAccountAggregateRoot { get; set; }

        protected List<Guid> CreatedGuids = new List<Guid>();

        protected Exception CaughtException { get; set; }

        protected IAggregateRootFactory AggregateRootFactory { get; private set; }

        protected AggregateRepositoryTestBase(string testFixtureOptions)
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

            containerBuilder.RegisterType<Calculator>().As<ICalculator>();
            containerBuilder.RegisterType<AccountAggregateRoot>();
            containerBuilder.RegisterType<NonDisposingAccountAggregateRoot>();

            var container = containerBuilder.Build();

            Subject = container.Resolve<IAggregateRepository<AccountAggregateRoot, Guid>>();
            AggregateRootFactory = container.Resolve<IAggregateRootFactory>();
            AggregateId = CreateAggregateId();
        }

        protected Guid CreateAggregateId()
        {
            var guid = Guid.NewGuid();
            CreatedGuids.Add(guid);
            return guid;
        }

        protected override void Cleanup()
        {
            Subject.Delete(AggregateId);
        }
    }
}