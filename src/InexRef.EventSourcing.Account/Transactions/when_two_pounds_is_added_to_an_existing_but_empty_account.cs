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
    public class when_two_pounds_is_added_to_an_existing_but_empty_account : AccountDomainTestBase
    {
        private MakeDepositCommand _makeDepositCommand;
        private CreateAccountCommand _createAccountCommand;

        public when_two_pounds_is_added_to_an_existing_but_empty_account(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            await Subject.Send(_createAccountCommand = new CreateAccountCommand(MessageMetadata.CreateDefault(), NaturalId));
            RecordedMessages.Clear();
        }

        protected override async Task When() => 
            await Subject.Send(_makeDepositCommand = new MakeDepositCommand(MessageMetadata.CreateDefault(), NaturalId, MonetaryAmount.Create(2.00M)));

        [Then]
        public async Task the_account_balance_is_two_pounds() => (await Repository.GetByNaturalKey(NaturalId)).Balance.ShouldBe(Balance.FromDecimal(2.0M));

        [Then]
        public void the_account_balance_on_the_read_model_is_two_pounds() => BalanceReadModel[NaturalId].ShouldBe(2.00M);

        [Then]
        public void only_two_event_were_sent_in_response_to_the_deposit_action()
            => RecordedMessages.RecordedEvents.ShouldHaveACountOf(2);

        [Then]
        public void an_event_was_sent_indicating_the_deposit_of_two_pounds()
            => RecordedMessages
                .ContainsEvent<DepositMadeEvent>()
                .GeneratedBy(_makeDepositCommand)
                .Where(@event => @event.Amount == _makeDepositCommand.Amount);

        [Then]
        public void an_event_was_sent_indicating_the_balance_had_changed_to_two_pounds()
            => RecordedMessages
                .ContainsEvent<BalanceUpdatedEvent>()
                .GeneratedBy(_makeDepositCommand)
                .Where(@event => @event.Balance == Balance.FromDecimal(2));
    }
}
