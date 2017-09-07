using System;

namespace Rob.EventSourcing.Messages
{
    public interface ICommand<TId> : ICommand where TId : IEquatable<TId>, IComparable<TId>
    {
        
    }

    public interface ICommand : IMessage
    {

    }
}