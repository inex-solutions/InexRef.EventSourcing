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
        IAmReadModelFor<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent>
    {
        public decimal UnauditedPrice { get; set; }

        public DateTime PriceDateTime { get; set; }

        public string Currency { get; set; }

        [MsSqlReadModelIdentityColumn]
        public string ValuationLineId { get; set; }

        public string AggregateId { get; set; }

        public DateTimeOffset? CreateTime { get; set; }

        public DateTimeOffset UpdatedTime { get; set; }

        public int SequenceNumber { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent> domainEvent)
        {
            var priceDateTime = domainEvent.AggregateEvent.UnauditedPrice.PriceDateTime;
            if (priceDateTime > PriceDateTime)
            {
                UnauditedPrice = domainEvent.AggregateEvent.UnauditedPrice.Value;
                PriceDateTime = domainEvent.AggregateEvent.UnauditedPrice.PriceDateTime;
                SequenceNumber = domainEvent.AggregateSequenceNumber;
                Currency = domainEvent.AggregateEvent.UnauditedPrice.Currency;
                ValuationLineId = domainEvent.AggregateEvent.UnauditedPrice.Id;
                AggregateId = domainEvent.AggregateIdentity.Value;
                
                UpdatedTime = DateTime.Now;

                if (CreateTime == null)
                {
                    CreateTime = UpdatedTime;
                }
            }
        }
    }
}