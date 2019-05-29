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