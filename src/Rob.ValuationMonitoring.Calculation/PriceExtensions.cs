using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation
{
    public static class PriceExtensions
    {
        public static UpdateUnauditedPriceCommand ToUpdateUnauditedPriceCommand(this UnauditedPrice price, ValuationLineId valuationLineId, string name)
        {
            return new UpdateUnauditedPriceCommand(valuationLineId, name, price.PriceDateTime, price.Currency, price.Value, price.AsOfDateTime);
        }

        public static UpdateAuditedPriceCommand ToUpdateAuditedPriceCommand(this AuditedPrice price, ValuationLineId valuationLineId, string name)
        {
            return new UpdateAuditedPriceCommand(valuationLineId, name, price.PriceDateTime, price.Currency, price.Value, price.AsOfDateTime);
        }
    }
}