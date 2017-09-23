using System;
using Rob.EventSourcing.Contracts;

namespace Rob.EventSourcing.NaturalKey
{
    public interface INaturalKeyDrivenAggregateRepository<TAggregate, TInternalId, TNaturalKey>
        where TAggregate : IAggregateRoot<TInternalId>, IAggregateRootInternal<TInternalId>, new()
        where TInternalId : IEquatable<TInternalId>, IComparable<TInternalId>
        where TNaturalKey : IEquatable<TNaturalKey>, IComparable<TNaturalKey>
    {
        void DeleteByNaturalKey(TNaturalKey key);
        TAggregate GetByNaturalKey(TNaturalKey id);
        TAggregate GetOrCreateNewByNaturalKey(TNaturalKey naturalKey, Action<TAggregate> onCreateNew);
        void Save(TAggregate aggregate);
    }
}