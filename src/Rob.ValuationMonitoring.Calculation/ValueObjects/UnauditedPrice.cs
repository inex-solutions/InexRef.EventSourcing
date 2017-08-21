using System;

namespace Rob.ValuationMonitoring.Calculation.ValueObjects
{
    public class UnauditedPrice : Price
    {
        public UnauditedPrice(string id, DateTime priceDateTime, string currency, decimal value, DateTime asOfDateTime) : base(id, priceDateTime, currency, value, asOfDateTime)
        {
        }
    }
}