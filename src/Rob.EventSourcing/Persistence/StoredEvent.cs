using System;
using Rob.ValuationMonitoring.EventSourcing.Messages;

namespace Rob.ValuationMonitoring.EventSourcing.Persistence
{
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
}