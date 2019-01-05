using System.Collections.Generic;
using System.Threading.Tasks;

namespace InexRef.EventSourcing.Contracts.Persistence
{
    public interface INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId, TAggregate>
    {
        Task<TInternalId> this[TNaturalKey naturalKey] { get; }

        Task<TInternalId> CreateNew(TNaturalKey naturalKey);

        Task<TInternalId> GetOrCreateNew(TNaturalKey naturalKey);

        Task Delete(TNaturalKey naturalKey);

        IEnumerable<TNaturalKey> GetAllKeys();
    }
}