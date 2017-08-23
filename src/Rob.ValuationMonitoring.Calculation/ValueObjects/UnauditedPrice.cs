using System;

namespace Rob.ValuationMonitoring.Calculation.ValueObjects
{
    public class UnauditedPrice : Price
    {
        public UnauditedPrice(DateTime priceDateTime, string currency, decimal value, DateTime asOfDateTime) : base(priceDateTime, currency, value, asOfDateTime)
        {
        }
    }
}