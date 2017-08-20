using System;
using EventFlow.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.ValueObjects
{
    public abstract class Price : ValueObject
    {
        public Price(string id, DateTime priceDateTime, string currency, decimal value)
        {
            Id = id;
            PriceDateTime = priceDateTime;
            Currency = currency;
            Value = value;
        }

        public string Id { get; }
        public DateTime PriceDateTime { get; }
        public string Currency { get; }
        public decimal Value { get; }

        public override string ToString() => $"{GetType().Name} for {Id}: {Currency}{Value}@{PriceDateTime}";
    }
}