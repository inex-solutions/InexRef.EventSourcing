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

using Rob.EventSourcing.Tests.IntegrationTests;

namespace Rob.EventSourcing.Tests
{
    public class AccountAggregateRoot : AggregateRoot<string>
    {
        public AccountAggregateRoot()
        {
            
        }

        public AccountAggregateRoot(string id)
        {
            Id = id;
        }

        public decimal Balance { get; set; }

        public void AddAmount(decimal amount)
        {
            Apply(new AmountAddedEvent(Id, amount));
        }

        public void ResetBalance()
        {
            Apply(new BalanceResetEvent(Id));
        }

        public void HandleEvent(AmountAddedEvent @event, bool isNew)
        {
            Balance += @event.Amount;
            PublishEvent(new BalanceUpdatedEvent(Id, Balance), isNew);
        }

        public void HandleEvent(BalanceResetEvent @event, bool isNew)
        {
            Balance = 0;
            PublishEvent(new BalanceResetEvent(Id), isNew);
        }
    }
}