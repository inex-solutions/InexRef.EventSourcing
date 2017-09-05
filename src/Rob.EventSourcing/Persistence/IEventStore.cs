using System;
using System.Collections.Generic;
using Rob.ValuationMonitoring.EventSourcing.Messages;

namespace Rob.ValuationMonitoring.EventSourcing.Persistence
{
    public interface IEventStore
    {
        IEnumerable<Event> LoadEvents(Guid aggregateId);
        bool TryLoadEvents(Guid aggregateId, out IEnumerable<Event> events);
        void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
    }
}