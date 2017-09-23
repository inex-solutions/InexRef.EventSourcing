using System;
using Rob.EventSourcing.Contracts;
using Rob.EventSourcing.Contracts.Persistence;

namespace Rob.EventSourcing.NaturalKey
{
    public class NaturalKeyDrivenAggregateRepository<TAggregate, TInternalId, TNaturalKey> : INaturalKeyDrivenAggregateRepository<TAggregate, TInternalId, TNaturalKey>
        where TAggregate : IAggregateRoot<TInternalId>, IAggregateRootInternal<TInternalId>, new()
        where TInternalId : IEquatable<TInternalId>, IComparable<TInternalId>
        where TNaturalKey : IEquatable<TNaturalKey>, IComparable<TNaturalKey>
    {
        private readonly IAggregateRepository<TAggregate, TInternalId> _internalRepository;
        private readonly INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId> _naturalKeyToAggregateIdMap;

        public NaturalKeyDrivenAggregateRepository(
            IAggregateRepository<TAggregate, TInternalId> internalRepository,
            INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId> naturalKeyToAggregateIdMap)
        {
            _internalRepository = internalRepository;
            _naturalKeyToAggregateIdMap = naturalKeyToAggregateIdMap;
        }

        public TAggregate GetByNaturalKey(TNaturalKey id)
        {
            return _internalRepository.Get(_naturalKeyToAggregateIdMap[id]);
        }

        public TAggregate GetOrCreateNewByNaturalKey(TNaturalKey naturalKey, Action<TAggregate> onCreateNew)
        {
            TInternalId id = _naturalKeyToAggregateIdMap.GetOrCreateNew(naturalKey);
            return _internalRepository.GetOrCreateNew(id, onCreateNew);
        }

        public void Save(TAggregate aggregate)
        {
            _internalRepository.Save(aggregate);
        }

        public void DeleteByNaturalKey(TNaturalKey key)
        {
            _internalRepository.Delete(_naturalKeyToAggregateIdMap[key]);
        }
    }
}