using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation
{
    public class ValuationLineAggregate : AggregateRoot<ValuationLineAggregate, ValuationLineId>
    {
        public ValuationLineAggregate(ValuationLineId id) : base(id)
        {
        }

        public void UpdateUnauditedPrice(UnauditedPrice price)
        {
            
        }
    }
}
