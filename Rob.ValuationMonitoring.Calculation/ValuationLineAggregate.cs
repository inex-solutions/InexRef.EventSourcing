using System;
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
        public UnauditedPrice LastUnauditedPrice { get; private set; }

        public ValuationLineAggregate(ValuationLineId id) : base(id)
        {
        }

        public void UpdateUnauditedPrice(UnauditedPrice price)
        {
            Console.WriteLine($"UpdateUnauditedPrice: {price}");
            Emit(new UnauditedPriceReceivedEvent(price));
        }

        public void Apply(UnauditedPriceReceivedEvent aggregateEvent)
        {
            LastUnauditedPrice = aggregateEvent.UnauditedPrice;
        }
    }
}
