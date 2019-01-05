using System;
using System.Collections.Generic;

namespace InexRef.EventSourcing.Contracts.Messages
{
    public abstract class Event<TId> : IEvent<TId>, IEventInternal, IEquatable<Event<TId>> where TId : IEquatable<TId>, IComparable<TId>
    {
        protected Event(MessageMetadata messageMetadata, TId id)
        {
            MessageMetadata = messageMetadata;
            Id = id;
        }

        public TId Id { get; }

        public int Version { get; set; }

        public MessageMetadata MessageMetadata { get; }

        void IEventInternal.SetVersion(int version)
        {
            Version = version;
        }

        public bool Equals(Event<TId> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TId>.Default.Equals(Id, other.Id) && Version == other.Version && Equals(MessageMetadata, other.MessageMetadata);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Event<TId>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<TId>.Default.GetHashCode(Id);
                hashCode = (hashCode * 397) ^ Version;
                hashCode = (hashCode * 397) ^ (MessageMetadata != null ? MessageMetadata.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {Id} - v{Version}: {MessageMetadata}";
        }
    }
}