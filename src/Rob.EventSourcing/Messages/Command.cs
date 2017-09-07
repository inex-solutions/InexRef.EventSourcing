using System;

namespace Rob.EventSourcing.Messages
{
    public abstract class Command : ICommand
    {
        protected Command(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}