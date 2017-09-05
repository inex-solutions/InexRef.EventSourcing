using System;

namespace Rob.EventSourcing.Messages
{
    public abstract class Event : IMessage
    {
        public Guid Id { get; }

        protected Event(Guid id)
        {
            Id = id;
        }

        public int Version { get; set; }
    }
}
