using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InexRef.EventSourcing.Contracts.Persistence
{
    public interface INaturalKeyDrivenAggregateRepository<TAggregate, TInternalId, TNaturalKey>
        where TAggregate : IAggregateRoot<TInternalId>, IAggregateRootInternal<TInternalId>
        where TInternalId : IEquatable<TInternalId>, IComparable<TInternalId>
        where TNaturalKey : IEquatable<TNaturalKey>, IComparable<TNaturalKey>
    {
        Task DeleteByNaturalKey(TNaturalKey key);
        Task<TAggregate> GetByNaturalKey(TNaturalKey id);
        Task<TAggregate> CreateNewByNaturalKey(TNaturalKey naturalKey, Action<TAggregate> onCreateNew);
        Task<TAggregate> GetOrCreateNewByNaturalKey(TNaturalKey naturalKey, Action<TAggregate> onCreateNew);
        Task Save(TAggregate aggregate);
        IEnumerable<TNaturalKey> GetAllKeys();
    }
}