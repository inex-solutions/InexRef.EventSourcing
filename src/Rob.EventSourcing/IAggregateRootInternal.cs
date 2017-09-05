using System;
using System.Collections.Generic;
using Rob.ValuationMonitoring.EventSourcing.Messages;

namespace Rob.ValuationMonitoring.EventSourcing
{
    public interface IAggregateRootInternal
    {
        void Load(Guid id, IEnumerable<Event> history);
    }
}