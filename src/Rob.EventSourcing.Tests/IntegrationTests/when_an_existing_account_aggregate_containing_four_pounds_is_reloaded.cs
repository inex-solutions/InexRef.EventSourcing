#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017 INEX Solutions Ltd
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

using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class when_an_existing_account_aggregate_containing_four_pounds_is_reloaded : IntegrationTestBase
    {
        private AccountAggregateRoot Aggregate { get; set; }

        public when_an_existing_account_aggregate_containing_four_pounds_is_reloaded(string persistenceProvider) : base(persistenceProvider) { }

        protected override void Given()
        {
            Subject.Send(new AddAmountCommand(AggregateId, 2.00M));
            Subject.Send(new AddAmountCommand(AggregateId, 2.00M));
            ReceivedEventsHistoryReadModel.Clear();
        }

        protected override void When() => Aggregate = Repository.Get(AggregateId);

        [Then]
        public void the_reloaded_aggregate_has_a_balance_of_four_pounds() => Aggregate.Balance.ShouldBe(4.0m);

        [Then]
        public void the_reloaded_aggregate_has_is_version_2() => Aggregate.Version.ShouldBe(2);

        [Then]
        public void no_updates_were_received_by_the_read_model() => ReceivedEventsHistoryReadModel[AggregateId].ShouldBeNull();
    }
}