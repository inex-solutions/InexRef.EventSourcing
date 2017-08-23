using System;
using EventFlow.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateAuditedPriceCommand : Command<ValuationLineAggregate, ValuationLineId>
    {
        public UpdateAuditedPriceCommand(ValuationLineId aggregateId, string id, DateTime priceDateTime, string currency, decimal value, DateTime asOfDateTime) 
            : base(aggregateId)
        {
            Id = id;
            PriceDateTime = priceDateTime;
            Currency = currency;
            Value = value;
            AsOfDateTime = asOfDateTime;
        }

        public string Id { get; }
        public DateTime PriceDateTime { get; }
        public DateTime AsOfDateTime { get; }
        public string Currency { get; }
        public decimal Value { get; }
    }
}