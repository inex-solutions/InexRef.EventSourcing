using System;

namespace Rob.EventSourcing.Messages
{
    public abstract class Event<TId> : IEvent<TId>, IEventInternal where TId : IEquatable<TId>, IComparable<TId>
    {
        public TId Id { get; }

        public int Version { get; set; }

        protected Event(TId id)
        {
            Id = id;
        }

        void IEventInternal.SetVersion(int version)
        {
            Version = version;
        }
    }
}
