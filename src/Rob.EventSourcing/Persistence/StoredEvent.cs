using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    internal class StoredEvent<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        public int Version { get; }

        public TId AggregateId { get; }

        public Event EventData { get; }

        public StoredEvent(TId aggregateId, int version, Event eventData)
        {
            AggregateId = aggregateId;
            Version = version;
            EventData = eventData;
        }
    }
}