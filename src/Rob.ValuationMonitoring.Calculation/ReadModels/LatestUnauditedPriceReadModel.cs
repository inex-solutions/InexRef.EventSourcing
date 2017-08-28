using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.MsSql.ReadStores.Attributes;
using EventFlow.ReadStores;
using Rob.ValuationMonitoring.Calculation.Events;

namespace Rob.ValuationMonitoring.Calculation.ReadModels
{
    [Table("ReadModel-LatestUnauditedPrice")]
    public class LatestUnauditedPriceReadModel : 
        IReadModel,
        IAmReadModelFor<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent>,
        IAmReadModelFor<ValuationLineAggregate, ValuationLineId, AuditedPriceReceivedEvent>,
        IAmReadModelFor<ValuationLineAggregate, ValuationLineId, ValuationLineNameChangedEvent>
    {
        public decimal UnauditedPrice { get; set; }

        public DateTime EffectiveDateTime { get; set; }

        public string Currency { get; set; }

        [MsSqlReadModelIdentityColumn]
        public string ValuationLineId { get; set; }

        public DateTimeOffset? CreateTime { get; set; }

        public DateTimeOffset UpdatedTime { get; set; }

        public int SequenceNumber { get; set; }

        public string ValuationLineName { get; set; }

        public DateTime ValuationLineNameEffectiveDateTime { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent> domainEvent)
        {
            // this logic should not be here - it only exists because we are subscribing to *input* events of the aggregate
            var priceDateTime = domainEvent.AggregateEvent.UnauditedPrice.PriceDateTime;
            if (priceDateTime > EffectiveDateTime)
            {
                UnauditedPrice = domainEvent.AggregateEvent.UnauditedPrice.Value;
                EffectiveDateTime = domainEvent.AggregateEvent.UnauditedPrice.PriceDateTime;
                SequenceNumber = domainEvent.AggregateSequenceNumber;
                Currency = domainEvent.AggregateEvent.UnauditedPrice.Currency;
                ValuationLineId = domainEvent.AggregateIdentity.Value;
                
                UpdatedTime = DateTime.Now;

                if (CreateTime == null)
                {
                    CreateTime = UpdatedTime;
                }
            }
        }

        public void Apply(IReadModelContext context, IDomainEvent<ValuationLineAggregate, ValuationLineId, AuditedPriceReceivedEvent> domainEvent)
        {
            // this logic should not be here - it only exists because we are subscribing to *input* events of the aggregate
            var priceDateTime = domainEvent.AggregateEvent.AuditedPrice.PriceDateTime;
            if (priceDateTime > EffectiveDateTime)
            {
                UnauditedPrice = domainEvent.AggregateEvent.AuditedPrice.Value;
                EffectiveDateTime = domainEvent.AggregateEvent.AuditedPrice.PriceDateTime;
                SequenceNumber = domainEvent.AggregateSequenceNumber;
                Currency = domainEvent.AggregateEvent.AuditedPrice.Currency;
                ValuationLineId = domainEvent.AggregateIdentity.Value;

                UpdatedTime = DateTime.Now;

                if (CreateTime == null)
                {
                    CreateTime = UpdatedTime;
                }
            }
        }

        public void Apply(IReadModelContext context, IDomainEvent<ValuationLineAggregate, ValuationLineId, ValuationLineNameChangedEvent> domainEvent)
        {
            // this logic should not be here - it only exists because we are subscribing to *input* events of the aggregate
            if (domainEvent.AggregateEvent.NameEffectiveDateTime > ValuationLineNameEffectiveDateTime)
            {
                ValuationLineNameEffectiveDateTime = domainEvent.AggregateEvent.NameEffectiveDateTime;
                ValuationLineName = domainEvent.AggregateEvent.Name;
            }
        }
    }
}