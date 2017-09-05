using System;

namespace Rob.EventSourcing.Messages
{
    public abstract class Command : IMessage
    {
        protected Command(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}