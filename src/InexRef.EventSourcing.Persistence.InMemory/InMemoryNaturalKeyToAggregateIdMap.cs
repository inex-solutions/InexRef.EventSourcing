using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Persistence;

namespace InexRef.EventSourcing.Persistence.InMemory
{
    public class InMemoryNaturalKeyToAggregateIdMap<TNaturalKey, TInternalId, TAggregate> : INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId, TAggregate>

    {
        private readonly IAggregateIdCreator<TInternalId> _aggregateIdCreator;
        private readonly ConcurrentDictionary<TNaturalKey, TInternalId> _keyMap = new ConcurrentDictionary<TNaturalKey, TInternalId>();

        public Task<TInternalId> this[TNaturalKey naturalKey] => Task.FromResult(_keyMap[naturalKey]);

        public InMemoryNaturalKeyToAggregateIdMap(IAggregateIdCreator<TInternalId> aggregateIdCreator)
        {
            _aggregateIdCreator = aggregateIdCreator;
        }

        public Task<TInternalId> CreateNew(TNaturalKey naturalKey)
        {
            var internalId = _aggregateIdCreator.Create();

            if (!_keyMap.TryAdd(naturalKey, internalId))
            {
                throw new InvalidOperationException($"Key already exists: {naturalKey}");
            }

            return Task.FromResult(internalId);
        }

        public Task<TInternalId> GetOrCreateNew(TNaturalKey naturalKey)
        {
            return Task.FromResult(_keyMap.GetOrAdd(key: naturalKey, valueFactory: key => _aggregateIdCreator.Create()));
        }

        public Task Delete(TNaturalKey naturalKey)
        {
            return Task.FromResult(_keyMap.TryRemove(naturalKey, out TInternalId removedItem));
        }

        public IEnumerable<TNaturalKey> GetAllKeys()
        {
            return _keyMap.Keys.AsEnumerable();
        }
    }
}