using System;

namespace InexRef.EventSourcing.Contracts.Messages
{
    public abstract class Command<TId> : ICommand<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        protected Command(MessageMetadata messageMetadata, TId id)
        {
            MessageMetadata = messageMetadata;
            Id = id;
        }

        public MessageMetadata MessageMetadata { get; }

        public TId Id { get; }
    }
}