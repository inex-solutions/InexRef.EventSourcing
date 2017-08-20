using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.MsSql.ReadStores;
using EventFlow.ReadStores;
using Rob.ValuationMonitoring.Calculation.Events;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.ReadModels
{
    [Table("ReadModel-ValuationLine")]
    public class ValuationLineReadModel : 
        MssqlReadModel,
        IAmReadModelFor<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent>
    {
        public decimal UnauditedPrice { get; set; }

        public DateTime PriceDateTime { get; set; }

        public string Currency { get; set; }

        public string ValuationLineId { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<ValuationLineAggregate, ValuationLineId, UnauditedPriceReceivedEvent> domainEvent)
        {
            UnauditedPrice = domainEvent.AggregateEvent.UnauditedPrice.Value;
            PriceDateTime = domainEvent.AggregateEvent.UnauditedPrice.PriceDateTime;
            Currency = domainEvent.AggregateEvent.UnauditedPrice.Currency;
            ValuationLineId = domainEvent.AggregateIdentity.Value;
        }
    }
}