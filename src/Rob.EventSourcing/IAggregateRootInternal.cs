using System;
using System.Collections.Generic;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing
{
    public interface IAggregateRootInternal<TId> : IDisposable where TId : IEquatable<TId>, IComparable<TId>
    {
        void Load(TId id, IEnumerable<IEvent<TId>> history);

        IEnumerable<IEvent<TId>> GetUncommittedEvents();

        IEnumerable<IEvent<TId>> GetUnpublishedEvents();
    }
}