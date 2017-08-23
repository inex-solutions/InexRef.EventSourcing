using EventFlow.Aggregates;

namespace Rob.ValuationMonitoring.Calculation.Events
{
    public class ValuationLineCreatedEvent : IAggregateEvent<ValuationLineAggregate, ValuationLineId>
    {
        public ValuationLineCreatedEvent(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code { get; }

        public string Name { get; }
    }
}
