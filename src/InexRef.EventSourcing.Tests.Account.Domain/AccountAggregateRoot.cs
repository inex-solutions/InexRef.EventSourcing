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

using System;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Domain;
using InexRef.EventSourcing.Tests.Account.Contract.Public.Messages;
using InexRef.EventSourcing.Tests.Account.Contract.Public.Types;

namespace InexRef.EventSourcing.Tests.Account.Domain
{
    public class AccountAggregateRoot : AggregateRoot<Guid>
    {
        private readonly ICalculator _calculator;

        public AccountAggregateRoot(ICalculator calculator)
        {
            _calculator = calculator;
            Balance = Balance.Zero;
            AccountId = AccountId.Null;
        }

        public AccountId AccountId { get; private set; }

        public override string Name => "Account";

        public Balance Balance { get; private set; }

        public void InitialiseAccount(MessageMetadata messageMetadata, Guid id, AccountId accountId)
        {
            Apply(new AccountInitialisedEvent(messageMetadata, id, accountId));
        }

        public void AddAmount(MessageMetadata messageMetadata, MonetaryAmount amount)
        {
            Apply(new AmountAddedEvent(messageMetadata, Id, amount));
        }

        public void ResetBalance(MessageMetadata messageMetadata)
        {
            Apply(new BalanceResetEvent(messageMetadata, Id));
        }

        public void HandleEvent(AccountInitialisedEvent @event, bool isNew)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
        }

        public void HandleEvent(AmountAddedEvent @event, bool isNew)
        {
            Balance = _calculator.AddToBalance(Balance, @event.Amount);
            Apply(new BalanceUpdatedEvent(MessageMetadata.CreateFromMessage(@event),  Id, AccountId, Balance), isNew);
        }

        public void HandleEvent(BalanceResetEvent @event, bool isNew)
        {
            Balance = Balance.Zero;
            Apply(new BalanceUpdatedEvent(MessageMetadata.CreateFromMessage(@event), Id, AccountId, Balance), isNew);
        }
    }
}