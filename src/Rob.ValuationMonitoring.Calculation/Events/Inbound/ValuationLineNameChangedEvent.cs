using System;
using EventFlow.Aggregates;

namespace Rob.ValuationMonitoring.Calculation.Events.Inbound
{
    public class ValuationLineNameChangedEvent : IAggregateEvent<ValuationLineAggregate, ValuationLineId>
    {
        public string Name { get; }
        public DateTime NameEffectiveDateTime { get; }

        public ValuationLineNameChangedEvent(string name, DateTime nameEffectiveDateTime)
        {
            Name = name;
            NameEffectiveDateTime = nameEffectiveDateTime;
        }
    }
}