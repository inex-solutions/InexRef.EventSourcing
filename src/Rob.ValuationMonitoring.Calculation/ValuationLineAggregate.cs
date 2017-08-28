using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Snapshots;
using EventFlow.Snapshots.Strategies;
using Rob.ValuationMonitoring.Calculation.Events;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation
{
    public class ValuationLineAggregate :
        SnapshotAggregateRoot<ValuationLineAggregate, ValuationLineId, ValuationLineSnapshot>
    {
        private ValuationLineState _state = new ValuationLineState();

        public ValuationLineAggregate(ValuationLineId id) : base(id, SnapshotEveryFewVersionsStrategy.With(100))
        {
            Register(_state);
        }

        public string ValuationLineName => _state.ValuationLineName;

        public UnauditedPrice LastUnauditedPrice => _state.LastUnauditedPrice;

        public Price ReferencePrice => _state.ReferencePrice;

        public decimal ValuationChange => _state.ValuationChange;

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

        protected override Task<ValuationLineSnapshot> CreateSnapshotAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_state.ToSnapshot());
        }

        protected override Task LoadSnapshotAsync(ValuationLineSnapshot snapshot, ISnapshotMetadata metadata, CancellationToken cancellationToken)
        {
            _state.UpdateFromSnapshot(snapshot);

            return Task.FromResult(0);
        }
    }
}
