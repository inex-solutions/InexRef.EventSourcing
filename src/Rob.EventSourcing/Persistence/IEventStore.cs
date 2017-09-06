using System;
using System.Collections.Generic;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public interface IEventStore
    {
        IEnumerable<Event> LoadEvents(Guid aggregateId);
        bool TryLoadEvents(Guid aggregateId, out IEnumerable<Event> events);
        void SaveEvents(Guid id, Type aggregateType, IEnumerable<Event> events, int currentVersion, int expectedVersion);
        void DeleteEvents(Guid id, Type aggregateType);
    }
}