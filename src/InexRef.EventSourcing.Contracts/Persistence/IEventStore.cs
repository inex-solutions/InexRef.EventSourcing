using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Contracts.Persistence
{
    public interface IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        IEnumerable<IEvent<TId>> LoadEvents(TId aggregateId, Type aggregateType, bool throwIfNotFound);
        Task SaveEvents(TId id, Type aggregateType, IEnumerable<IEvent<TId>> events, int currentVersion, int expectedVersion);
        Task DeleteEvents(TId id, Type aggregateType);
    }
}