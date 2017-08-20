﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.Events;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation
{
    public class ValuationLineAggregate : 
        AggregateRoot<ValuationLineAggregate, ValuationLineId>,
        IEmit<UnauditedPriceReceivedEvent>
    {
        private decimal _price;

        public ValuationLineAggregate(ValuationLineId id) : base(id)
        {
        }

        public void UpdateUnauditedPrice(UnauditedPrice price)
        {
            Console.WriteLine($"UpdateUnauditedPrice: {price}");
            Emit(new UnauditedPriceReceivedEvent(price.Value));
        }

        public void Apply(UnauditedPriceReceivedEvent aggregateEvent)
        {
            _price = aggregateEvent.UnauditedPrice;
        }
    }
}
