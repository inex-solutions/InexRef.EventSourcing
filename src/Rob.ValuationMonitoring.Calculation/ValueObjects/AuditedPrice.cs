using System;
using EventFlow.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.ValueObjects
{
    public class AuditedPrice : Price
    {
        public AuditedPrice(string id, DateTime priceDateTime, string currency, decimal value) : base(id, priceDateTime, currency, value)
        {
        }
    }
}