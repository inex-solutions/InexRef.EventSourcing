﻿using System;
using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.Events;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation
{
    public class ValuationLineAggregate : 
        AggregateRoot<ValuationLineAggregate, ValuationLineId>,
        IEmit<ValuationLineCreatedEvent>,
        IEmit<UnauditedPriceReceivedEvent>,
        IEmit<AuditedPriceReceivedEvent>
    {
        public string ValuationLineCode { get; private set; }

        public string ValuationLineName { get; private set; }

        public Price ReferencePrice { get; private set; }

        public UnauditedPrice LastUnauditedPrice { get; private set; }

        public decimal ValuationChange { get; private set; }

        public ValuationLineAggregate(ValuationLineId id) : base(id)
        {
        }

        public void OnCreate(string valuationLineCode, string valuationLineName)
        {
            Emit(new ValuationLineCreatedEvent(valuationLineCode, valuationLineName));
        }

        public void UpdateUnauditedPrice(UnauditedPrice price)
        {
            Emit(new UnauditedPriceReceivedEvent(price));
        }

        public void UpdateAuditedPrice(AuditedPrice price)
        {
            Emit(new AuditedPriceReceivedEvent(price));
        }

        public void Apply(UnauditedPriceReceivedEvent aggregateEvent)
        {
            var unauditedPrice = aggregateEvent.UnauditedPrice;

            if (LastUnauditedPrice ==  null
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

        public void Apply(ValuationLineCreatedEvent aggregateEvent)
        {
            //ValuationLineCode = aggregateEvent.Code;
            //ValuationLineName = aggregateEvent.Name;
        }
    }
}
