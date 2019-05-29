#region Copyright & Licence
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
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