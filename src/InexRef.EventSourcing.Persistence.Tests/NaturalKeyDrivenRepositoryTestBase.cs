#region Copyright & Licence
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
            containerBuilder.ConfigureFrom<EventSourcingCoreModule>();
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