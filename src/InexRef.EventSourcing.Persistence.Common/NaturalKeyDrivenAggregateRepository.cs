using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Persistence;

namespace InexRef.EventSourcing.Persistence.Common
{
    public class NaturalKeyDrivenAggregateRepository<TAggregate, TInternalId, TNaturalKey> : INaturalKeyDrivenAggregateRepository<TAggregate, TInternalId, TNaturalKey>
        where TAggregate : IAggregateRoot<TInternalId>, IAggregateRootInternal<TInternalId>
        where TInternalId : IEquatable<TInternalId>, IComparable<TInternalId>
        where TNaturalKey : IEquatable<TNaturalKey>, IComparable<TNaturalKey>
    {
        private readonly IAggregateRepository<TAggregate, TInternalId> _internalRepository;
        private readonly INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId, TAggregate> _naturalKeyToAggregateIdMap;

        public NaturalKeyDrivenAggregateRepository(
            IAggregateRepository<TAggregate, TInternalId> internalRepository,
            INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId, TAggregate> naturalKeyToAggregateIdMap)
        {
            _internalRepository = internalRepository;
            _naturalKeyToAggregateIdMap = naturalKeyToAggregateIdMap;
        }

        public async Task<TAggregate> GetByNaturalKey(TNaturalKey id)
        {
            return await _internalRepository.Get(await _naturalKeyToAggregateIdMap[id]);
        }

        public async Task<TAggregate> GetOrCreateNewByNaturalKey(TNaturalKey naturalKey, Action<TAggregate> onCreateNew)
        {
            TInternalId id = await _naturalKeyToAggregateIdMap.GetOrCreateNew(naturalKey);
            return await _internalRepository.GetOrCreateNew(id, onCreateNew);
        }

        public async Task<TAggregate> CreateNewByNaturalKey(TNaturalKey naturalKey, Action<TAggregate> onCreateNew)
        {
            TInternalId id = await _naturalKeyToAggregateIdMap.GetOrCreateNew(naturalKey);
            return await _internalRepository.GetOrCreateNew(id, onCreateNew);
        }

        public async Task Save(TAggregate aggregate)
        {
            await _internalRepository.Save(aggregate);
        }

        public IEnumerable<TNaturalKey> GetAllKeys()
        {
            return _naturalKeyToAggregateIdMap.GetAllKeys();
        }

        public async Task DeleteByNaturalKey(TNaturalKey key)
        {
            await _internalRepository.Delete(await _naturalKeyToAggregateIdMap[key]);
            await _naturalKeyToAggregateIdMap.Delete(key);
        }
    }
}