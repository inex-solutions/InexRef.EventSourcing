﻿#region Copyright & License
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
    public class when_the_balance_is_reset_on_an_account_containing_two_pounds : IntegrationTestBase
    {
        public when_the_balance_is_reset_on_an_account_containing_two_pounds(string testFixtureOptions) : base(testFixtureOptions) { }

        protected override void Given()
        {
            Subject.Send(new AddAmountCommand(AccountId, 2.00M));
        }

        protected override void When() => Subject.Send(new ResetBalanceCommand(AccountId));

        [Then]
        public void the_account_balance_is_zero() => Repository.GetByNaturalKey(AccountId).Balance.ShouldBe(0.00M);

        [Then]
        public void the_account_balance_on_the_read_model_is_zero() => BalanceReadModel[AccountId].ShouldBe(0.00M);

        [Then]
        public void the_aggregate_is_version_three() => Repository.GetByNaturalKey(AccountId).Version.ShouldBe(3);

        [Then]
        public void the_version_on_the_read_model_subscribed_to_external_events_is_three() => BalanceReadModel.GetVersion(AccountId).ShouldBe(3);
    }
}