using System;
using System.Collections.Generic;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public interface IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        IEnumerable<IEvent<TId>> LoadEvents(TId aggregateId);
        bool TryLoadEvents(TId aggregateId, out IEnumerable<IEvent<TId>> events);
        void SaveEvents(TId id, Type aggregateType, IEnumerable<IEvent<TId>> events, int currentVersion, int expectedVersion);
        void DeleteEvents(TId id, Type aggregateType);
    }
}