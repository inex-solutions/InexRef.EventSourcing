using System;
using EventFlow.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateUnauditedPriceCommand : Command<ValuationLineAggregate, ValuationLineId>
    {
        public UpdateUnauditedPriceCommand(ValuationLineId aggregateId, string code, string name, DateTime priceDateTime, string currency, decimal value, DateTime asOfDateTime)
            : base(aggregateId)
        {
            Code = code;
            Name = name;
            PriceDateTime = priceDateTime;
            Currency = currency;
            Value = value;
            AsOfDateTime = asOfDateTime;
        }

        public string Code { get; }
        public string Name { get; }
        public DateTime PriceDateTime { get; }
        public DateTime AsOfDateTime { get; }
        public string Currency { get; }
        public decimal Value { get; }
    }
}