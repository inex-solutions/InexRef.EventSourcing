using System;
using System.Collections.Generic;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing
{
    public interface IAggregateRootInternal : IDisposable
    {
        void Load(Guid id, IEnumerable<Event> history);

        IEnumerable<Event> GetUncommittedEvents();
        IEnumerable<Event> GetUnpublishedEvents();

        void ClearUncommittedEvents();
        void ClearUnpublishedEvents();
    }
}