using System;
using System.Threading.Tasks;

namespace InexRef.EventSourcing.Contracts.Persistence
{
    public interface IAggregateRepository<TAggregate, in TId> 
        where TAggregate : IAggregateRoot<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        Task Save(TAggregate aggregate);

        Task<TAggregate> Get(TId id);

        Task<TAggregate> GetOrCreateNew(TId id, Action<TAggregate> onCreateNew);

        Task Delete(TId id);
    }
}