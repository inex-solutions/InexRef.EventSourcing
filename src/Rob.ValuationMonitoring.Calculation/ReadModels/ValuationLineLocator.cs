using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Rob.ValuationMonitoring.Calculation.Events;

namespace Rob.ValuationMonitoring.Calculation.ReadModels
{
    public class ValuationLineLocator : IReadModelLocator
    {
        public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent)
        {
            var messageAddedEvent = domainEvent as IDomainEvent<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent>;
            if (messageAddedEvent == null)
            {
                yield break;
            }

            yield return messageAddedEvent.AggregateEvent.UnauditedPrice.Id;
        }
    }
}