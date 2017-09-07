using System;

namespace Rob.EventSourcing.Messages
{
    public abstract class Command<TId> : ICommand<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        protected Command(TId id)
        {
            Id = id;
        }

        public TId Id { get; }
    }
}