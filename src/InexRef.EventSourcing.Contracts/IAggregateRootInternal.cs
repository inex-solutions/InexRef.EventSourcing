using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Contracts
{
    public interface IAggregateRootInternal<TId> : IDisposable where TId : IEquatable<TId>, IComparable<TId>
    {
        Task Load(TId id, IEnumerable<IEvent<TId>> history);

        IEnumerable<IEvent<TId>> GetUncommittedEvents();
    }
}