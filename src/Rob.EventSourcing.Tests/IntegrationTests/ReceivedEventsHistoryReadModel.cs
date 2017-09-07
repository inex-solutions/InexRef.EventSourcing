using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class ReceivedEventsHistoryReadModel
    {
        private readonly ConcurrentDictionary<Guid, List<Event>> _receivedEvents = new ConcurrentDictionary<Guid, List<Event>>();

        public void Handle(BalanceUpdatedEvent @event)
        {
            ReceiveEvent(@event);
        }

        public void Handle(BalanceResetEvent @event)
        {
            ReceiveEvent(@event);
        }

        private void ReceiveEvent(Event @event)
        {
            _receivedEvents.AddOrUpdate(
                key: @event.Id,
                addValue: new List<Event>(new [] {@event}), 
                updateValueFactory: (id, entry) => entry.Concat(new [] {@event}).ToList());
        }

        public void Clear()
        {
            _receivedEvents.Clear();
        }

        public IList<Event> this[Guid id]
        {
            get
            {
                List<Event> events = null;
                _receivedEvents.TryGetValue(id, out events);
                return events;
            }
        }

    }
}
