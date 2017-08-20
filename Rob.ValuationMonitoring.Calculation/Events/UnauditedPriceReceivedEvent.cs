using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFlow.Aggregates;

namespace Rob.ValuationMonitoring.Calculation.Events
{
    public class UnauditedPriceReceivedEvent : IAggregateEvent<ValuationLineAggregate, ValuationLineId>
    {
        public decimal UnauditedPrice { get; }

        public UnauditedPriceReceivedEvent(decimal unauditedPrice)
        {
            UnauditedPrice = unauditedPrice;
        }
    }
}
