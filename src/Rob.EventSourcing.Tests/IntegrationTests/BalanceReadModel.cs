using System;
using System.Collections.Concurrent;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class BalanceReadModel
    {
        private readonly ConcurrentDictionary<string, BalanceEntry> _balances = new ConcurrentDictionary<string, BalanceEntry>();

        public void Handle(BalanceUpdatedEvent @event)
        {
            _balances.AddOrUpdate(
                key: @event.Id,
                addValue: new BalanceEntry {Version = @event.Version, Balance = @event.Balance},
                updateValueFactory: (id, entry) => @event.Version < entry.Version
                    ? entry
                    : new BalanceEntry {Version = @event.Version, Balance = @event.Balance});
        }

        public void Handle(BalanceResetEvent @event)
        {
            _balances.AddOrUpdate(
                key: @event.Id,
                addValue: new BalanceEntry { Version = @event.Version, Balance = 0 },
                updateValueFactory: (id, entry) => @event.Version < entry.Version
                    ? entry
                    : new BalanceEntry { Version = @event.Version, Balance = 0 });
        }

        public decimal this[string id] => _balances[id].Balance;

        public int GetVersion(string id) => _balances[id].Version;
    }
}