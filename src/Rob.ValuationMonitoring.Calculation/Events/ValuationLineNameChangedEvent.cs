using EventFlow.Aggregates;

namespace Rob.ValuationMonitoring.Calculation.Events
{
    public class ValuationLineNameChangedEvent : IAggregateEvent<ValuationLineAggregate, ValuationLineId>
    {
        public string Name { get; }

        public ValuationLineNameChangedEvent(string name)
        {
            Name = name;
        }
    }
}