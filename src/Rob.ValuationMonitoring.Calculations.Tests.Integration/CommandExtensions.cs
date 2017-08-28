using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration
{
    public static class CommandExtensions 
    {
        public static UnauditedPrice ToUnauditedPrice(this UpdateUnauditedPriceCommand command)
        {
            return new UnauditedPrice(command.EffectiveDateTime, command.Currency, command.Value, command.AsOfDateTime);
        }

        public static AuditedPrice ToAuditedPrice(this UpdateAuditedPriceCommand command)
        {
            return new AuditedPrice(command.EffectiveDateTime, command.Currency, command.Value, command.AsOfDateTime);
        }
    }
}