using System;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence
{
    public interface IAggregateRepository<TAggregate> where TAggregate : AggregateRoot, new()
    {
        void Save(TAggregate aggregate, int expectedVersion);

        TAggregate GetById(Guid id);
    }
}