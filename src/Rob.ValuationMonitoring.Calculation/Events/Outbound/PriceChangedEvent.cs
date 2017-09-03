using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Events.Outbound
{
    public class PriceChangedEvent : IAggregateEvent<ValuationLineAggregate, ValuationLineId>
    {
        public Price Price { get; }

        public PriceChangedEvent(Price price)
        {
            Price = price;
        }
    }
}