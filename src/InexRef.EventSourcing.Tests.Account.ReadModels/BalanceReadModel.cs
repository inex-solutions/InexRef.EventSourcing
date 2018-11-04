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

using System.Collections.Concurrent;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Tests.Account.Messages;

namespace InexRef.EventSourcing.Tests.Account.ReadModels
{
    public class BalanceReadModel : IHandle<AccountInitialisedEvent>, IHandle<BalanceUpdatedEvent>
    {
        private readonly ConcurrentDictionary<string, BalanceEntry> _balances =
            new ConcurrentDictionary<string, BalanceEntry>();

        public void Handle(AccountInitialisedEvent @event)
        {
            _balances.TryAdd(@event.AccountId, new BalanceEntry {Version = @event.Version, Balance = 0});
        }

        public void Handle(BalanceUpdatedEvent @event)
        {
            _balances[@event.AccountId] = new BalanceEntry {Version = @event.Version, Balance = @event.Balance};
        }

        public decimal this[string id] => _balances[id].Balance;

        public int GetVersion(string id) => _balances[id].Version;

        private class BalanceEntry
        {
            public decimal Balance { get; set; }

            public int Version { get; set; }
        }
    }
}