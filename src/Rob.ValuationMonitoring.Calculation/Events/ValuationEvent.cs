using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Events
{
    public class ValuationEvent : IAggregateEvent<ValuationLineAggregate, ValuationLineId>
    {
        public UnauditedPrice UnauditedPrice { get; }

        public ValuationEvent(UnauditedPrice unauditedPrice)
        {
            UnauditedPrice = unauditedPrice;
        }
    }
}
