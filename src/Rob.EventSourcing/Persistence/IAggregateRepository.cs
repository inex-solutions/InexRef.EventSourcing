using System;

namespace Rob.EventSourcing.Persistence
{
    public interface IAggregateRepository<TAggregate> where TAggregate : AggregateRoot, new()
    {
        void Save(TAggregate aggregate);

        TAggregate Get(Guid id);

        TAggregate GetOrCreateNew(Guid id);
        void Delete(Guid id);
    }
}