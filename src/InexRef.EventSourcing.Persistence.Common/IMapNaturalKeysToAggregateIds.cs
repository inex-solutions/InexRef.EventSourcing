using System;

namespace InexRef.EventSourcing.Persistence.Common
{
    public interface IMapNaturalKeysToAggregateIds<in TNaturalKey, out TInternalId> where TInternalId : IEquatable<TInternalId>, IComparable<TInternalId>
    {
        TInternalId GetOrCreateNew(TNaturalKey naturalKey);

        TInternalId this[TNaturalKey id] { get; }
    }
}