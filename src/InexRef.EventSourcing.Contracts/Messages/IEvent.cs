using System;

namespace InexRef.EventSourcing.Contracts.Messages
{
    public interface IEvent : IMessage
    {
        int Version { get; }
    }

    public interface IEvent<out TId> : IEvent where TId : IEquatable<TId>, IComparable<TId>
    {
        TId Id { get; }
    }
}