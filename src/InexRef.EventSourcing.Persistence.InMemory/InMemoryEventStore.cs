using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Persistence.Common;

namespace InexRef.EventSourcing.Persistence.InMemory
{
    public class InMemoryEventStore<TId> : IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly ConcurrentDictionary<TId, IEnumerable<StoredEvent<TId>>> _storedEvents = new ConcurrentDictionary<TId, IEnumerable<StoredEvent<TId>>>();

        public async Task SaveEvents(TId aggregateId, Type aggregateType, IEnumerable<IEvent<TId>> events, int currentVersion, int expectedVersion)
        {
            IEnumerable<StoredEvent<TId>> eventsToStore = events.Select(@event => new StoredEvent<TId>(@event.Version, @event)).ToList();

            _storedEvents.AddOrUpdate(
                key: aggregateId,
                addValue: eventsToStore,
                updateValueFactory: (id, enumerable) =>
                {
                    var persistedEvents = enumerable.ToList();
                    var version = persistedEvents.LastOrDefault()?.Version ?? 0;
                    if (version != expectedVersion)
                    {
                        throw new EventStoreConcurrencyException($"Concurrency error saving aggregate {id} (expected version {expectedVersion}, actual {version})");
                    }

                    var eventsToStoreList = eventsToStore.ToList();

                    return persistedEvents.Concat(eventsToStoreList);
                });

            await Task.CompletedTask;
        }

        public async Task DeleteEvents(TId id, Type aggregateType)
        {
            _storedEvents.TryRemove(id, out IEnumerable<StoredEvent<TId>>  events);
            await Task.CompletedTask;
        }

        public IEnumerable<IEvent<TId>> LoadEvents(TId aggregateId, Type aggregateType, bool throwIfNotFound)
        {
            IEnumerable<StoredEvent<TId>> events;

            if (!_storedEvents.TryGetValue(aggregateId, out events))
            {
                if (throwIfNotFound)
                {
                    throw new AggregateNotFoundException($"Aggregate '{aggregateId}' not found");
                }

                return Enumerable.Empty<IEvent<TId>>();
            }

            return events.Select(@event => @event.EventData);
        }

        private class StoredEvent<T> where T : IEquatable<T>, IComparable<T>
        {
            internal int Version { get; }

            internal IEvent<T> EventData { get; }

            internal StoredEvent(int version, IEvent<T> eventData)
            {
                Version = version;
                EventData = eventData;
            }
        }
    }
}