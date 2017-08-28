using System;
using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.Events;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation
{
    public class ValuationLineAggregate : 
        AggregateRoot<ValuationLineAggregate, ValuationLineId>
    {
        private readonly ValuationLineState _state = new ValuationLineState();

        public ValuationLineAggregate(ValuationLineId id) : base(id)
        {
            Register(_state);
        }

        public string ValuationLineName => _state.ValuationLineName;

        public UnauditedPrice LastUnauditedPrice => _state.LastUnauditedPrice;

        public Price ReferencePrice => _state.ReferencePrice;

        public decimal ValuationChange => _state.ValuationChange;

        public void UpdateUnauditedPrice(UnauditedPrice price)
        {
            Emit(new UnauditedPriceReceivedEvent(price));
        }

        public void UpdateAuditedPrice(AuditedPrice price)
        {
            Emit(new AuditedPriceReceivedEvent(price));
        }

        public void UpdateValuationLineName(string valuationLineName, DateTime effectiveDate)
        {
            Emit(new ValuationLineNameChangedEvent(valuationLineName, effectiveDate));
        }
    }
}
