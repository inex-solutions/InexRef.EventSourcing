using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Contracts.Messages;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class ReceivedInternalEventsHistoryReadModel
    {
        private readonly ConcurrentDictionary<string, List<IEvent<string>>> _receivedEvents = new ConcurrentDictionary<string, List<IEvent<string>>>();

        public void Handle(AmountAddedEvent @event)
        {
            ReceiveEvent(@event);
        }

        public void Handle(BalanceResetEvent @event)
        {
            ReceiveEvent(@event);
        }

        private void ReceiveEvent(IEvent<string> @event)
        {
            _receivedEvents.AddOrUpdate(
                key: @event.Id,
                addValue: new List<IEvent<string>>(new[] { @event }),
                updateValueFactory: (id, entry) => entry.Concat(new[] { @event }).ToList());
        }

        public void Clear()
        {
            _receivedEvents.Clear();
        }

        public IList<IEvent<string>> this[string id]
        {
            get
            {
                List<IEvent<string>> events = null;
                _receivedEvents.TryGetValue(id, out events);
                return events;
            }
        }
    }
}