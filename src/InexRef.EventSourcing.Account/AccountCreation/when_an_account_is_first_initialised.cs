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

namespace InexRef.EventSourcing.Account.DomainHost.Tests.AccountCreation
{
    public class when_an_account_is_first_initialised : AccountDomainTestBase
    {
        private CreateAccountCommand _createAccountCommand;
        public when_an_account_is_first_initialised(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task When() => 
            await Subject.Send(_createAccountCommand = new CreateAccountCommand(MessageMetadata.CreateDefault(), NaturalId));

        [Then]
        public async Task the_account_balance_is_zero() => (await Repository.GetByNaturalKey(NaturalId)).Balance.ShouldBe(Balance.FromDecimal(0.0M));

        [Then]
        public void the_account_balance_on_the_read_model_is_zero() => BalanceReadModel[NaturalId].ShouldBe(0.00M);

        [Then]
        public void only_one_event_was_sent()
            => RecordedMessages.RecordedEvents.ShouldHaveACountOf(1);

        [Then]
        public void an_event_was_sent_for_account_initialisation()
            => RecordedMessages
                .ContainsEvent<AccountInitialisedEvent>()
                .GeneratedBy(_createAccountCommand);
    }
}