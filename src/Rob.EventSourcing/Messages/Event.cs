using System;

namespace Rob.EventSourcing.Messages
{
    public abstract class Event : IMessage
    {
        public Guid Id { get; }

        public int Version { get; set; }

        protected Event(Guid id)
        {
            Id = id;
        }
    }
}
