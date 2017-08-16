using EventFlow.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateUnauditedPriceCommand : Command<ValuationLineAggregate, ValuationLineId>
    {
        public UpdateUnauditedPriceCommand(
            ValuationLineId aggregateId,
            UnauditedPrice unauditedPrice)
            : base(aggregateId)
        {
            UnauditedPrice = unauditedPrice;
        }

        public UnauditedPrice UnauditedPrice { get; }
    }
}