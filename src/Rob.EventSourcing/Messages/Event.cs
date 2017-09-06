using System;

namespace Rob.EventSourcing.Messages
{
    public abstract class Event : IMessage, IEventInternal
    {
        public Guid Id { get; }

        public int Version { get; set; }

        protected Event(Guid id)
        {
            Id = id;
        }

        void IEventInternal.SetVersion(int version)
        {
            Version = version;
        }
    }
}
