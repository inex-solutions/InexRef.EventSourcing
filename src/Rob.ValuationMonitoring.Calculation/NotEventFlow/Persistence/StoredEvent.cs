using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence
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