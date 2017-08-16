using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFlow.Aggregates;

namespace Rob.ValuationMonitoring.Calculation
{
    public class ValuationLineAggregate : AggregateRoot<ValuationLineAggregate, ValuationLineId>
    {
        public ValuationLineAggregate(ValuationLineId id) : base(id)
        {
        }
    }
}
