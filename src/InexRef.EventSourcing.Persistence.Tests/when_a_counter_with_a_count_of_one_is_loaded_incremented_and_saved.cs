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

using System.Threading.Tasks;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    public class when_a_counter_with_a_count_of_one_is_loaded_incremented_and_saved : AggregateRepositoryTestBase
    {
        public when_a_counter_with_a_count_of_one_is_loaded_incremented_and_saved(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            var aggregate = AggregateRootFactory.Create<CounterAggregateRoot>();
            await aggregate.Initialise(AggregateId);
            await aggregate.Increment();
            await Subject.Save(aggregate);

            ReloadedCounterAggregateRoot = await Subject.Get(AggregateId);
            await ReloadedCounterAggregateRoot.Increment();
            await Subject.Save(ReloadedCounterAggregateRoot);
        }

        protected override async Task When() => ReloadedCounterAggregateRoot = await Subject.Get(AggregateId);

        [Then]
        public void then_the_counter_has_a_value_of_two_when_reloaded() => ReloadedCounterAggregateRoot.CurrentValue.ShouldBe(2);
    }
}