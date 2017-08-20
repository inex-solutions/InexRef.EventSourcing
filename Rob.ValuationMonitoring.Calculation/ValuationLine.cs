using EventFlow.Entities;

namespace Rob.ValuationMonitoring.Calculation
{
    public class ValuationLine : Entity<ValuationLineId>
    {
        public decimal UnauditedPrice { get; }

        public ValuationLine(
            ValuationLineId id,
            decimal unauditedPrice)
            : base(id)
        {
            UnauditedPrice = unauditedPrice;
        }
    }
}