using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IEventBus _eventBus;
        private readonly ConcurrentDictionary<Guid, IEnumerable<StoredEvent>> _storedEvents = new ConcurrentDictionary<Guid, IEnumerable<StoredEvent>>();

        public InMemoryEventStore(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void SaveEvents(Guid aggregateId, Type aggregateType, IEnumerable<Event> events, int expectedVersion)
        {
            int eventVersion = expectedVersion;
            IEnumerable<StoredEvent> eventsToStore = events.Select(@event =>
            {
                @event.Version = ++eventVersion;
                return new StoredEvent(aggregateId, @event.Version, @event);
            }).ToList();

            _storedEvents.AddOrUpdate(
                key: aggregateId,
                addValue: eventsToStore,
                updateValueFactory: (id, enumerable) =>
                {
                    var version = enumerable.LastOrDefault()?.Version ?? 0;
                    if (version != expectedVersion)
                    {
                        throw new EventStoreConcurrencyException($"Concurrency error saving aggregate {id} (expected version {expectedVersion}, actual {version})");
                    }

                    var eventsToStoreList = eventsToStore.ToList();

                    //HACK: what if we fail to send? We've already saved. Here's the tricky bit :)
                    foreach (var eventToStore in eventsToStoreList)
                    {
                        _eventBus.PublishEvent(eventToStore.EventData);
                    }

                    return enumerable.Concat(eventsToStoreList);
                });
        }

        public void DeleteEvents(Guid id, Type aggregateType)
        {
            IEnumerable<StoredEvent> events;
            _storedEvents.TryRemove(id, out events);
        }

        public bool TryLoadEvents(Guid aggregateId, out IEnumerable<Event> events)
        {
            IEnumerable<StoredEvent> storedEvents;

            if (!_storedEvents.TryGetValue(aggregateId, out storedEvents))
            {
                events = null;
                return false;
            }

            events = storedEvents.Select(@event => @event.EventData);
            return true;
        }

        public IEnumerable<Event> LoadEvents(Guid aggregateId)
        {
            IEnumerable<StoredEvent> events;

            if (!_storedEvents.TryGetValue(aggregateId, out events))
            {
                throw new AggregateNotFoundException($"Aggregate '{aggregateId}' not found");
            }

            return events.Select(@event => @event.EventData);
        }
    }
}