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
    public class when_two_pounds_is_added_to_a_non_existent_account : IntegrationTestBase
    {
        public when_two_pounds_is_added_to_a_non_existent_account(string persistenceProvider) : base(persistenceProvider) { }

        protected override void Given() { }

        protected override void When() => Subject.Send(new AddAmountCommand(AggregateId, 2.00M));

        [Then]
        public void a_new_account_is_created_with_a_balance_of_two_pounds() => Repository.Get(AggregateId).Balance.ShouldBe(2.00M);

        [Then]
        public void the_account_balance_on_the_read_model_is_two_pounds() => BalanceReadModel[AggregateId].ShouldBe(2.00M);

        [Then]
        public void the_aggregate_is_version_one() => Repository.Get(AggregateId).Version.ShouldBe(1);

        [Then]
        public void the_version_on_the_read_model_is_one() => BalanceReadModel.GetVersion(AggregateId).ShouldBe(1);

        [Then]
        public void the_total_number_of_updates_received_by_a_subscribed_read_model_is_one() => ReceivedEventsHistoryReadModel[AggregateId].ShouldHaveACountOf(1);

        [Then]
        public void the_number_of_events_received_by_the_read_model_subscribed_to_internal_events_is_one() 
            => ReceivedInternalEventsHistoryReadModel[AggregateId].ShouldHaveACountOf(1);
    }
}