using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Events
{
    public class AuditedPriceReceivedEvent : IAggregateEvent<ValuationLineAggregate, ValuationLineId>
    {
        public AuditedPrice AuditedPrice { get; }

        public AuditedPriceReceivedEvent(AuditedPrice auditedPrice)
        {
            AuditedPrice = auditedPrice;
        }
    }
}