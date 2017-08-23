using System;
using EventFlow.Commands;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateUnauditedPriceCommand : Command<ValuationLineAggregate, ValuationLineId>
    {
        public UpdateUnauditedPriceCommand(ValuationLineId id, DateTime priceDateTime, string currency, decimal value, DateTime asOfDateTime)
            : base(id)
        {
            PriceDateTime = priceDateTime;
            Currency = currency;
            Value = value;
            AsOfDateTime = asOfDateTime;
        }

        public DateTime PriceDateTime { get; }
        public DateTime AsOfDateTime { get; }
        public string Currency { get; }
        public decimal Value { get; }
    }
}