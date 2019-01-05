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

using System.Threading.Tasks;
using InexRef.EventSourcing.Persistence.Common;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    public class when_a_saved_counter_is_resaved_but_with_the_wrong_version_number : AggregateRepositoryTestBase
    {
        public when_a_saved_counter_is_resaved_but_with_the_wrong_version_number(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            ReloadedCounterAggregateRoot = AggregateRootFactory.Create<NonDisposingCounterAggregateRoot>();
            await ReloadedCounterAggregateRoot.Initialise(AggregateId);

            await ReloadedCounterAggregateRoot.Increment();

            // intermediate save should cause a concurrency error when we save below
            await Subject.Save(ReloadedCounterAggregateRoot);

            await ReloadedCounterAggregateRoot.Increment();
        }

        protected override async Task When() => CaughtException = await Catch.AsyncException (async () => await Subject.Save(ReloadedCounterAggregateRoot));

        [Then]
        public void a_EventStoreConcurrencyException_should_be_thrown() => CaughtException.ShouldBeOfType<EventStoreConcurrencyException>();
    }
}