using System.Collections.Generic;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow
{
    public interface IAggregateRootInternal
    {
        void Load(IEnumerable<Event> history);
    }
}