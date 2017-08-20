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
        private readonly List<UnauditedPrice> _prices = new List<UnauditedPrice>();

        public IReadOnlyCollection<UnauditedPrice> Prices => _prices;

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
            _prices.Add(aggregateEvent.UnauditedPrice);
        }
    }
}
