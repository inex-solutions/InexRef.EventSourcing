using System;
using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.Events;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation
{
    public class ValuationLineState : 
        AggregateState<ValuationLineAggregate, ValuationLineId, ValuationLineState>,
        IApply<UnauditedPriceReceivedEvent>,
        IApply<AuditedPriceReceivedEvent>,
        IApply<ValuationLineNameChangedEvent>
    {
        //temporary workaround for illustrative purposes
        public DateTime ValuationLineNameEffectiveDateTime { get; private set; }

        public string ValuationLineName { get; private set; }

        public Price ReferencePrice { get; private set; }

        public UnauditedPrice LastUnauditedPrice { get; private set; }

        public decimal ValuationChange { get; private set; }

        public void Apply(ValuationLineNameChangedEvent aggregateEvent)
        {
            if (aggregateEvent.NameEffectiveDateTime > ValuationLineNameEffectiveDateTime)
            {
                ValuationLineNameEffectiveDateTime = aggregateEvent.NameEffectiveDateTime;
                ValuationLineName = aggregateEvent.Name;
            }
        }

        public void Apply(UnauditedPriceReceivedEvent aggregateEvent)
        {
            var unauditedPrice = aggregateEvent.UnauditedPrice;

            if (LastUnauditedPrice == null
                || unauditedPrice.PriceDateTime > LastUnauditedPrice.PriceDateTime)
            {
                LastUnauditedPrice = unauditedPrice;
            }

            CalculateValuationChange();
        }

        public void Apply(AuditedPriceReceivedEvent aggregateEvent)
        {
            var auditedPrice = aggregateEvent.AuditedPrice;

            if (ReferencePrice == null
                || auditedPrice.PriceDateTime > ReferencePrice.PriceDateTime)
            {
                ReferencePrice = auditedPrice;
            }

            CalculateValuationChange();
        }

        private void CalculateValuationChange()
        {
            if (LastUnauditedPrice != null
                && ReferencePrice != null)
            {
                ValuationChange = (LastUnauditedPrice.Value - ReferencePrice.Value) / ReferencePrice.Value;
            }
        }
    }
}