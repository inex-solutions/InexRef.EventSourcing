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
            containerBuilder.ConfigureFrom<EventSourcingCoreModule>();
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