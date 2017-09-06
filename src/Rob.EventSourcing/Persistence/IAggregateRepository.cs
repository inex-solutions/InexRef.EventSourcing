using System;

namespace Rob.EventSourcing.Persistence
{
    public interface IAggregateRepository<TAggregate, TId> 
        where TAggregate : AggregateRoot<TId>, new() 
        where TId : IEquatable<TId>, IComparable<TId>
    {
        void Save(TAggregate aggregate);

        TAggregate Get(TId id);

        TAggregate GetOrCreateNew(TId id);

        void Delete(TId id);
    }
}