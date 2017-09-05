using System;

namespace Rob.ValuationMonitoring.EventSourcing.Persistence
{
    public interface IAggregateRepository<TAggregate> where TAggregate : AggregateRoot, new()
    {
        void Save(TAggregate aggregate, int expectedVersion);

        TAggregate Get(Guid id);

        TAggregate GetOrCreateNew(Guid id);
    }
}