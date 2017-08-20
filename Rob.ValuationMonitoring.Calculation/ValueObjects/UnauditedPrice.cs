using System;

namespace Rob.ValuationMonitoring.Calculation.ValueObjects
{
    public class UnauditedPrice
    {
        public UnauditedPrice(DateTime priceDateTime, string currency, decimal value)
        {
            PriceDateTime = priceDateTime;
            Currency = currency;
            Value = value;
        }

        public DateTime PriceDateTime { get; }
        public string Currency { get; }
        public decimal Value { get; }

        public override string ToString() => $"UnauditedPrice: {Currency}{Value}@{PriceDateTime}";
    }
}