using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.MsSql.ReadStores.Attributes;
using EventFlow.ReadStores;
using Rob.ValuationMonitoring.Calculation.Events;
using Rob.ValuationMonitoring.Calculation.Events.Outbound;

namespace Rob.ValuationMonitoring.Calculation.ReadModels
{
    [Table("ReadModel-LatestPriceOnly")]
    public class LatestPriceOnlyReadModel :
        IReadModel,
        IAmReadModelFor<ValuationLineAggregate, ValuationLineId, PriceChangedEvent>
    {
        public decimal UnauditedPrice { get; set; }

        [MsSqlReadModelIdentityColumn]
        public string ValuationLineId { get; set; }

        public DateTime EffectiveDateTime { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<ValuationLineAggregate, ValuationLineId, PriceChangedEvent> domainEvent)
        {
            EffectiveDateTime = domainEvent.AggregateEvent.Price.PriceDateTime;
            UnauditedPrice = domainEvent.AggregateEvent.Price.Value;
            ValuationLineId = domainEvent.AggregateIdentity.Value;
        }
    }
}