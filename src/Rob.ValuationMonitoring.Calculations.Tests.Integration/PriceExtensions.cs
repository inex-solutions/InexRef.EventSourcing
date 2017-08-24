using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration
{
    public static class PriceExtensions
    {
        public static UpdateUnauditedPriceCommand ToUpdateUnauditedPriceCommand(this UnauditedPrice price, ValuationLineId valuationLineId)
        {
            return new UpdateUnauditedPriceCommand(valuationLineId, $"InitialName-{valuationLineId}", price.PriceDateTime, price.Currency, price.Value, price.AsOfDateTime);
        }

        public static UpdateAuditedPriceCommand ToUpdateAuditedPriceCommand(this AuditedPrice price, ValuationLineId valuationLineId)
        {
            return new UpdateAuditedPriceCommand(valuationLineId, $"InitialName-{valuationLineId}", price.PriceDateTime, price.Currency, price.Value, price.AsOfDateTime);
        }
    }
}