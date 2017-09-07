using System;

namespace Rob.EventSourcing.Messages
{
    public interface IEvent : IMessage
    {
        int Version { get; }
    }

    public interface IEvent<TId> : IEvent where TId : IEquatable<TId>, IComparable<TId>
    {
        TId Id { get; }
    }
}