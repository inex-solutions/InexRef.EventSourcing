using System;
using System.Collections.Generic;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public interface IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        IEnumerable<Event> LoadEvents(TId aggregateId);
        bool TryLoadEvents(TId aggregateId, out IEnumerable<Event> events);
        void SaveEvents(TId id, Type aggregateType, IEnumerable<Event> events, int currentVersion, int expectedVersion);
        void DeleteEvents(TId id, Type aggregateType);
    }
}