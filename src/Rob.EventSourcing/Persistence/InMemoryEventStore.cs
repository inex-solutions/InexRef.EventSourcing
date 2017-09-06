using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public class InMemoryEventStore<TId> : IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly IEventBus _eventBus;
        private readonly ConcurrentDictionary<TId, IEnumerable<StoredEvent<TId>>> _storedEvents = new ConcurrentDictionary<TId, IEnumerable<StoredEvent<TId>>>();

        public InMemoryEventStore(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void SaveEvents(TId aggregateId, Type aggregateType, IEnumerable<Event> events, int currentVersion, int expectedVersion)
        {
            IEnumerable<StoredEvent<TId>> eventsToStore = events.Select(@event => new StoredEvent<TId>(aggregateId, @event.Version, @event)).ToList();

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

        public void DeleteEvents(TId id, Type aggregateType)
        {
            IEnumerable<StoredEvent<TId>> events;
            _storedEvents.TryRemove(id, out events);
        }

        public bool TryLoadEvents(TId aggregateId, out IEnumerable<Event> events)
        {
            IEnumerable<StoredEvent<TId>> storedEvents;

            if (!_storedEvents.TryGetValue(aggregateId, out storedEvents))
            {
                events = null;
                return false;
            }

            events = storedEvents.Select(@event => @event.EventData);
            return true;
        }

        public IEnumerable<Event> LoadEvents(TId aggregateId)
        {
            IEnumerable<StoredEvent<TId>> events;

            if (!_storedEvents.TryGetValue(aggregateId, out events))
            {
                throw new AggregateNotFoundException($"Aggregate '{aggregateId}' not found");
            }

            return events.Select(@event => @event.EventData);
        }
    }
}