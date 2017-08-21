using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Rob.ValuationMonitoring.Calculation.Events;

namespace Rob.ValuationMonitoring.Calculation.ReadModels
{
    public class RawEventReadModel : IReadModel,
        IAmReadModelFor<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent>
    {
        private readonly IList<UnauditedPriceReceivedEvent> _unauditedPriceReceivedEvents = new List<UnauditedPriceReceivedEvent>();

        public void Apply(IReadModelContext context, IDomainEvent<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent> domainEvent)
        {
            _unauditedPriceReceivedEvents.Add(domainEvent.AggregateEvent);
        }

        public IEnumerable<UnauditedPriceReceivedEvent> GetRawEventsAsOf(DateTime asOfDateTime)
        {
            return _unauditedPriceReceivedEvents.Where(e => e.UnauditedPrice.AsOfDateTime <= asOfDateTime);
        }
    }
}