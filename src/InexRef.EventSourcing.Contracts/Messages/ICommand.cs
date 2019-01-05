using System;

namespace InexRef.EventSourcing.Contracts.Messages
{
    public interface ICommand<out TId> : ICommand where TId : IEquatable<TId>, IComparable<TId>
    {
        TId Id { get; }
    }

    public interface ICommand : IMessage
    {

    }
}