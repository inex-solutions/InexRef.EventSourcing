using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Events
{
    public class UnauditedPriceReceivedEvent : IAggregateEvent<ValuationLineAggregate, ValuationLineId>
    {
        public UnauditedPrice UnauditedPrice { get; }

        public UnauditedPriceReceivedEvent(UnauditedPrice unauditedPrice)
        {
            UnauditedPrice = unauditedPrice;
        }
    }
}