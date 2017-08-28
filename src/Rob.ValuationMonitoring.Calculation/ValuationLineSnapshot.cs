using System;
using EventFlow.Snapshots;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation
{
    [SnapshotVersion("ValuationLine", 1)]
    public class ValuationLineSnapshot : ISnapshot
    {
        public DateTime ValuationLineNameEffectiveDateTime { get; set; }

        public string ValuationLineName { get; set; }

        public Price ReferencePrice { get; set; }

        public UnauditedPrice LastUnauditedPrice { get; set; }

        public decimal ValuationChange { get; set; }
    }
}