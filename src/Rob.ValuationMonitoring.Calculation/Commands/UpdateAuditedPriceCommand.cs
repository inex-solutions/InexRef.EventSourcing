using EventFlow.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateAuditedPriceCommand : Command<ValuationLineAggregate, ValuationLineId>
    {
        public UpdateAuditedPriceCommand(
            ValuationLineId aggregateId,
            AuditedPrice auditedPrice)
            : base(aggregateId)
        {
            AuditedPrice = auditedPrice;
        }

        public AuditedPrice AuditedPrice { get; }
    }
}