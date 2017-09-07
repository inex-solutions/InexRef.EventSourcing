using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    internal class StoredEvent<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        public int Version { get; }

        public TId AggregateId { get; }

        public IEvent<TId> EventData { get; }

        public StoredEvent(TId aggregateId, int version, IEvent<TId> eventData)
        {
            AggregateId = aggregateId;
            Version = version;
            EventData = eventData;
        }
    }
}