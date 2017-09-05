using System;
using System.Collections.Concurrent;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class BalanceReadModel
    {
        private readonly ConcurrentDictionary<Guid, BalanceEntry> _balances = new ConcurrentDictionary<Guid, BalanceEntry>();

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

        public decimal this[Guid id] => _balances[id].Balance;

        public int GetVersion(Guid id) => _balances[id].Version;
    }
}