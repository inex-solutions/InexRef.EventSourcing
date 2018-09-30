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

using InexRef.EventSourcing.Tests.Account.Domain;
using InexRef.EventSourcing.Tests.Account.Messages;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Shouldly;

namespace InexRef.EventSourcing.Tests.Account.DomainHost.Tests
{
    public class when_an_existing_account_aggregate_containing_four_pounds_is_reloaded : IntegrationTestBase
    {
        private AccountAggregateRoot Aggregate { get; set; }

        public when_an_existing_account_aggregate_containing_four_pounds_is_reloaded(string testFixtureOptions) : base(testFixtureOptions) { }

        protected override void Given()
        {
            Subject.Send(new AddAmountCommand(AccountId, 2.00M));
            Subject.Send(new AddAmountCommand(AccountId, 2.00M));
        }

        protected override void When() => Aggregate = Repository.GetByNaturalKey(AccountId);

        [Then]
        public void the_reloaded_aggregate_has_a_balance_of_four_pounds() => Aggregate.Balance.ShouldBe(Balance.FromDecimal(4.0M));

        [Then]
        public void the_reloaded_aggregate_version_is_at_least_3_being_the_initial_version_plus_two_actions() => Aggregate.Version.ShouldBeGreaterThanOrEqualTo(3);
    }
}