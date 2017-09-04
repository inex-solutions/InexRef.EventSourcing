using System;
using System.Collections.Generic;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow
{
    public interface IAggregateRootInternal
    {
        void Load(Guid id, IEnumerable<Event> history);
    }
}