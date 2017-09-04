using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EventFlow.Exceptions;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<Guid, IEnumerable<StoredEvent>> _storedEvents = new ConcurrentDictionary<Guid, IEnumerable<StoredEvent>>();

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
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
                    var version = enumerable.Last().Version;
                    if (version != expectedVersion)
                    {
                        throw new EventStoreConcurrencyException($"Concurrency error saving aggregate {id} (expected version {expectedVersion}, actual {version})");
                    }

                    return enumerable.Concat(eventsToStore);
                });
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

    internal class StoredEvent
    {
        public int Version { get; }

        public Guid AggregateId { get; }

        public Event EventData { get; }

        public StoredEvent(Guid aggregateId, int version, Event eventData)
        {
            AggregateId = aggregateId;
            Version = version;
            EventData = eventData;
        }
    }

    public class EventStoreConcurrencyException : Exception
    {
        public EventStoreConcurrencyException()
        {
            
        }

        public EventStoreConcurrencyException(string message) : base(message)
        {
            
        }
    }

    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException()
        {

        }

        public AggregateNotFoundException(string message) : base(message)
        {

        }
    }
}