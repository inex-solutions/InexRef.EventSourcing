using System;

namespace InexRef.EventSourcing.Contracts
{
    public interface IAggregateRoot<out TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        TId Id { get; }

        int Version { get; }

        string Name { get; }
    }
}