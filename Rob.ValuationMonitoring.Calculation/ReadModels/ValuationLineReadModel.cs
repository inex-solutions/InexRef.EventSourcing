using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Rob.ValuationMonitoring.Calculation.Events;

namespace Rob.ValuationMonitoring.Calculation.ReadModels
{
    public class ValuationLineReadModel : IReadModel,
        IAmReadModelFor<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent>
    {
        public decimal UnauditedPrice { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent> domainEvent)
        {
            UnauditedPrice = domainEvent.AggregateEvent.UnauditedPrice;
        }
    }
}