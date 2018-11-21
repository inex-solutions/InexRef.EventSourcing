#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2018 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

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