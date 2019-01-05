#region Copyright & License
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

using System;
using System.Threading.Tasks;
using InexRef.EventSourcing.Account.Contract.Public.Messages;
using InexRef.EventSourcing.Account.Contract.Public.Types;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.OperationContext;
using InexRef.EventSourcing.Domain;

namespace InexRef.EventSourcing.Account.Domain
{
    public class AccountAggregateRoot : AggregateRoot<Guid>
    {
        private readonly ICalculator _calculator;

        public AccountAggregateRoot(IOperationContext operationContext, ICalculator calculator) : base (operationContext)
        {
            _calculator = calculator;
            Balance = Balance.Zero;
            AccountId = AccountId.Null;

            RegisterEventHandler<AccountInitialisedEvent>(HandleEvent);
            RegisterEventHandler<AmountAddedEvent>(HandleEvent);
            RegisterEventHandler<BalanceResetEvent>(HandleEvent);
        }

        public AccountId AccountId { get; private set; }

        public override string Name => "Account";

        public Balance Balance { get; private set; }

        public async Task InitialiseAccount(MessageMetadata messageMetadata, Guid id, AccountId accountId)
        {
            await Apply(new AccountInitialisedEvent(messageMetadata, id, accountId));
        }

        public async Task AddAmount(MessageMetadata messageMetadata, MonetaryAmount amount)
        {
            await Apply(new AmountAddedEvent(messageMetadata, Id, amount));
        }

        public async Task ResetBalance(MessageMetadata messageMetadata)
        {
            await Apply(new BalanceResetEvent(messageMetadata, Id));
        }

        private async Task HandleEvent(AccountInitialisedEvent @event)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
            await Task.CompletedTask;
        }

        private async Task HandleEvent(AmountAddedEvent @event)
        {
            Balance = _calculator.AddToBalance(Balance, @event.Amount);
            await Apply(new BalanceUpdatedEvent(MessageMetadata.CreateFromMessage(@event),  Id, AccountId, Balance));
        }

        private async Task HandleEvent(BalanceResetEvent @event)
        {
            Balance = Balance.Zero;
            await Apply(new BalanceUpdatedEvent(MessageMetadata.CreateFromMessage(@event), Id, AccountId, Balance));
        }
    }
}