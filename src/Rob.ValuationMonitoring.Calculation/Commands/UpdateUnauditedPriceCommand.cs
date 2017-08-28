using System;
using EventFlow.Commands;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateUnauditedPriceCommand : Command<ValuationLineAggregate, ValuationLineId>
    {
        public UpdateUnauditedPriceCommand(ValuationLineId id, string name, DateTime effectiveDateTime, string currency, decimal value, DateTime asOfDateTime)
            : base(id)
        {
            Name = name;
            EffectiveDateTime = effectiveDateTime;
            Currency = currency;
            Value = value;
            AsOfDateTime = asOfDateTime;
        }

        public string Name { get; }
        public DateTime EffectiveDateTime { get; }
        public DateTime AsOfDateTime { get; }
        public string Currency { get; }
        public decimal Value { get; }
    }
}