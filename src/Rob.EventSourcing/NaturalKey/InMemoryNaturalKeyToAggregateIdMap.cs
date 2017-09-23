using System.Collections.Concurrent;

namespace Rob.EventSourcing.NaturalKey
{
    public class InMemoryNaturalKeyToAggregateIdMap<TNaturalKey, TInternalId> : INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId>

    {
        private readonly IAggregateIdCreator<TInternalId> _aggregateIdCreator;
        private readonly ConcurrentDictionary<TNaturalKey, TInternalId> _keyMap = new ConcurrentDictionary<TNaturalKey, TInternalId>();

        public TInternalId this[TNaturalKey naturalKey] => _keyMap[naturalKey];

        public InMemoryNaturalKeyToAggregateIdMap(IAggregateIdCreator<TInternalId> aggregateIdCreator)
        {
            _aggregateIdCreator = aggregateIdCreator;
        }

        public TInternalId GetOrCreateNew(TNaturalKey naturalKey)
        {
            return _keyMap.GetOrAdd(key: naturalKey, valueFactory: key => _aggregateIdCreator.Create());
        }
    }
}