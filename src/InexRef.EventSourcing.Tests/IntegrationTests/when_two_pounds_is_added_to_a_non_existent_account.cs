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

using InexRef.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace InexRef.EventSourcing.Tests.IntegrationTests
{
    public class when_two_pounds_is_added_to_a_non_existent_account : IntegrationTestBase
    {
        public when_two_pounds_is_added_to_a_non_existent_account(string testFixtureOptions) : base(testFixtureOptions) { }

        protected override void Given() { }

        protected override void When() => Subject.Send(new AddAmountCommand(AccountId, 2.00M));

        [Then]
        public void a_new_account_is_created_with_a_balance_of_two_pounds() => Repository.GetByNaturalKey(AccountId).Balance.ShouldBe(2.00M);

        [Then]
        public void the_account_balance_on_the_read_model_is_two_pounds() => BalanceReadModel[AccountId].ShouldBe(2.00M);

        [Then]
        public void the_aggregate_is_at_least_two_being_the_initial_version_plus_one_add_action() 
            => Repository.GetByNaturalKey(AccountId).Version.ShouldBeGreaterThanOrEqualTo(2);

        [Then]
        public void the_version_on_the_read_model_is_at_least_two_being_the_initial_version_plus_one_add_action() 
            => BalanceReadModel.GetVersion(AccountId).ShouldBeGreaterThanOrEqualTo(2);
    }
}