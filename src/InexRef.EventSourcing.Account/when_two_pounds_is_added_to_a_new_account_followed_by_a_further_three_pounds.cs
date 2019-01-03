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

using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Account.Contract.Public.Messages;
using InexRef.EventSourcing.Account.Contract.Public.Types;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Shouldly;

namespace InexRef.EventSourcing.Account.DomainHost.Tests
{
    public class when_two_pounds_is_added_to_a_new_account_followed_by_a_further_three_pounds : AccountDomainTestBase
    {
        public when_two_pounds_is_added_to_a_new_account_followed_by_a_further_three_pounds(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            await Subject.Send(new CreateAccountCommand(MessageMetadata.CreateDefault(), NaturalId));
            await Subject.Send(new AddAmountCommand(MessageMetadata.CreateDefault(), NaturalId, MonetaryAmount.Create(2.00M)));
        }

        protected override async Task When() => await Subject.Send(new AddAmountCommand(MessageMetadata.CreateDefault(), NaturalId, MonetaryAmount.Create(3.00M)));

        [Then]
        public async Task the_account_balance_is_five_pounds() 
            => (await Repository.GetByNaturalKey(NaturalId)).Balance.ShouldBe(Balance.FromDecimal(5.0M));

        [Then]
        public async Task the_aggregate_version_is_at_least_3_being_the_previous_version_plus_the_add_amount_action() 
            => (await Repository.GetByNaturalKey(NaturalId)).Version.ShouldBeGreaterThanOrEqualTo(3);

        [Then]
        public void the_account_balance_on_the_read_model_is_five_pounds() => BalanceReadModel[NaturalId].ShouldBe(5.00M);

        [Then]
        public void the_version_on_the_read_model_is_at_least_3_being_the_previous_version_plus_the_add_amount_action() 
            => BalanceReadModel.GetVersion(NaturalId).ShouldBeGreaterThanOrEqualTo(3);
    }
}