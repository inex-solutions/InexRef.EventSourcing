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
using InexRef.EventSourcing.Account.Contract.Public.Messages.Commands;
using InexRef.EventSourcing.Account.Contract.Public.Messages.Events;
using InexRef.EventSourcing.Account.Contract.Public.Types;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Tests.Common;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Shouldly;

namespace InexRef.EventSourcing.Account.DomainHost.Tests.Transactions
{
    public class when_a_three_pound_withdrawal_is_made_to_an_account_which_already_contains_eight_pounds : AccountDomainTestBase
    {
        private MakeWithdrawalCommand _makeWithdrawalCommand;

        public when_a_three_pound_withdrawal_is_made_to_an_account_which_already_contains_eight_pounds(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            await Subject.Send(new CreateAccountCommand(MessageMetadata.CreateDefault(), NaturalId));
            await Subject.Send(new MakeDepositCommand(MessageMetadata.CreateDefault(), NaturalId, MonetaryAmount.Create(8.00M)));
            RecordedMessages.Clear();
        }

        protected override async Task When()
            => await Subject.Send(_makeWithdrawalCommand = new MakeWithdrawalCommand(MessageMetadata.CreateDefault(), NaturalId, MonetaryAmount.Create(3.00M)));

        [Then]
        public async Task the_account_balance_is_five_pounds()
            => (await Repository.GetByNaturalKey(NaturalId)).Balance.ShouldBe(Balance.FromDecimal(5.0M));

        [Then]
        public async Task the_aggregate_version_is_at_least_3_being_the_previous_version_plus_the_withdrawal_action()
            => (await Repository.GetByNaturalKey(NaturalId)).Version.ShouldBeGreaterThanOrEqualTo(3);

        [Then]
        public void the_account_balance_on_the_read_model_is_five_pounds() => BalanceReadModel[NaturalId].ShouldBe(5.00M);

        [Then]
        public void the_version_on_the_read_model_is_at_least_3_being_the_previous_version_plus_the_withdrawal_action()
            => BalanceReadModel.GetVersion(NaturalId).ShouldBeGreaterThanOrEqualTo(3);

        [Then]
        public void only_two_event_were_sent_in_response_to_the_withdrawal_action()
            => RecordedMessages.RecordedEvents.ShouldHaveACountOf(2);

        [Then]
        public void an_event_was_sent_indicating_the_withdrawal_of_three_pounds()
            => RecordedMessages
                .ContainsEvent<WithdrawalMadeEvent>()
                .GeneratedBy(_makeWithdrawalCommand)
                .Where(@event => @event.Amount == _makeWithdrawalCommand.Amount);

        [Then]
        public void an_event_was_sent_indicating_the_balance_had_changed_to_five_pounds()
            => RecordedMessages
                .ContainsEvent<BalanceUpdatedEvent>()
                .GeneratedBy(_makeWithdrawalCommand)
                .Where(@event => @event.Balance == Balance.FromDecimal(5));
    }
}