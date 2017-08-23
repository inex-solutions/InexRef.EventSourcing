using System;
using EventFlow.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.ValueObjects
{
    public class AuditedPrice : Price
    {
        public AuditedPrice(DateTime priceDateTime, string currency, decimal value, DateTime asOfDateTime) : base(priceDateTime, currency, value, asOfDateTime)
        {
        }
    }
}