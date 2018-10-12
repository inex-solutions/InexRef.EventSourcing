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
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    public class when_an_aggregate_is_saved_but_then_subsequently_accessed : AggregateRepositoryTestBase
    {
        public when_an_aggregate_is_saved_but_then_subsequently_accessed(string testFixtureOptions) : base(testFixtureOptions) { }

        protected override void Given()
        {
            Aggregate = AggregateRootFactory.Create<CounterAggregateRoot>();
            Aggregate.Initialise(AggregateId);
            Subject.Save(Aggregate);
        }

        public CounterAggregateRoot Aggregate { get; set; }

        protected override void When() => CaughtException = Catch.Exception(() => Aggregate.Increment());

        [Then]
        public void an_ObjectDisposedException_is_thrown() => CaughtException.ShouldBeOfType<ObjectDisposedException>();
    }
}