using System;
using System.Collections.Generic;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence
{
    public interface IEventStore
    {
        IEnumerable<Event> LoadEvents(Guid aggregateId);
        void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
    }
}