using System;

namespace Rob.ValuationMonitoring.Calculation.ValueObjects
{
    public class UnauditedPrice : Price
    {
        public UnauditedPrice(string id, DateTime priceDateTime, string currency, decimal value) : base(id, priceDateTime, currency, value)
        {
        }
    }
}