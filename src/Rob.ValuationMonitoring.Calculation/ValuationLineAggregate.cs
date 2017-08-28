using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Snapshots;
using EventFlow.Snapshots.Strategies;
using Rob.ValuationMonitoring.Calculation.Events;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation
{
    public class ValuationLineAggregate :
        SnapshotAggregateRoot<ValuationLineAggregate, ValuationLineId, ValuationLineSnapshot>,
        IEmit<UnauditedPriceReceivedEvent>,
        IEmit<AuditedPriceReceivedEvent>,
        IEmit<ValuationLineNameChangedEvent>
    {
        //temporary workaround for illustrative purposes
        public DateTime ValuationLineNameEffectiveDateTime { get; private set; }

        public string ValuationLineName { get; private set; }

        public Price ReferencePrice { get; private set; }

        public UnauditedPrice LastUnauditedPrice { get; private set; }

        public decimal ValuationChange { get; private set; }

        public ValuationLineAggregate(ValuationLineId id) : base(id, SnapshotEveryFewVersionsStrategy.With(2))
        {
        }

        public void UpdateUnauditedPrice(UnauditedPrice price)
        {
            Emit(new UnauditedPriceReceivedEvent(price));
        }

        public void UpdateAuditedPrice(AuditedPrice price)
        {
            Emit(new AuditedPriceReceivedEvent(price));
        }

        public void UpdateValuationLineName(string valuationLineName, DateTime effectiveDate)
        {
            Emit(new ValuationLineNameChangedEvent(valuationLineName, effectiveDate));
        }

        public void Apply(ValuationLineNameChangedEvent aggregateEvent)
        {
            if (aggregateEvent.NameEffectiveDateTime > ValuationLineNameEffectiveDateTime)
            {
                ValuationLineNameEffectiveDateTime = aggregateEvent.NameEffectiveDateTime;
                ValuationLineName = aggregateEvent.Name;
            }
        }

        public void Apply(UnauditedPriceReceivedEvent aggregateEvent)
        {
            var unauditedPrice = aggregateEvent.UnauditedPrice;

            if (LastUnauditedPrice == null
                || unauditedPrice.PriceDateTime > LastUnauditedPrice.PriceDateTime)
            {
                LastUnauditedPrice = unauditedPrice;
            }

            CalculateValuationChange();
        }

        public void Apply(AuditedPriceReceivedEvent aggregateEvent)
        {
            var auditedPrice = aggregateEvent.AuditedPrice;

            if (ReferencePrice == null
                || auditedPrice.PriceDateTime > ReferencePrice.PriceDateTime)
            {
                ReferencePrice = auditedPrice;
            }

            CalculateValuationChange();
        }

        private void CalculateValuationChange()
        {
            if (LastUnauditedPrice != null
                && ReferencePrice != null)
            {
                ValuationChange = (LastUnauditedPrice.Value - ReferencePrice.Value) / ReferencePrice.Value;
            }
        }

        protected override Task<ValuationLineSnapshot> CreateSnapshotAsync(CancellationToken cancellationToken)
        {
            var snapshot = new ValuationLineSnapshot
            {
                ReferencePrice = ReferencePrice,
                LastUnauditedPrice = LastUnauditedPrice,
                ValuationLineName = ValuationLineName,
                ValuationChange = ValuationChange,
                ValuationLineNameEffectiveDateTime = ValuationLineNameEffectiveDateTime
            };

            return Task.FromResult(snapshot);
        }

        protected override Task LoadSnapshotAsync(ValuationLineSnapshot snapshot, ISnapshotMetadata metadata, CancellationToken cancellationToken)
        {
            ReferencePrice = snapshot.ReferencePrice;
            LastUnauditedPrice = snapshot.LastUnauditedPrice;
            ValuationLineName = snapshot.ValuationLineName;
            ValuationChange = snapshot.ValuationChange;
            ValuationLineNameEffectiveDateTime = snapshot.ValuationLineNameEffectiveDateTime;

            return Task.FromResult(0);
        }
    }
}
