using System;
using Newtonsoft.Json;

namespace InexRef.EventSourcing.Contracts.Messages
{
    public class MessageMetadata
    {
        [JsonConstructor]
        private MessageMetadata(string sourceCorrelationId, DateTime messageDateTime)
        {
            MessageDateTime = messageDateTime;
            SourceCorrelationId = sourceCorrelationId;
        }

        public string SourceCorrelationId { get; }

        public DateTime MessageDateTime { get; }

        public static MessageMetadata CreateEmpty()
            => new MessageMetadata(Guid.Empty.ToString(), DateTime.MinValue);

        public static MessageMetadata CreateDefault()
            => new MessageMetadata(Guid.NewGuid().ToString(), DateTime.UtcNow);

        public static MessageMetadata CreateFromMessage(IMessage @event)
            => new MessageMetadata(@event.MessageMetadata.SourceCorrelationId, DateTime.UtcNow);

        public static MessageMetadata Create(string sourceCorrelationId, DateTime messageDateTime)
            => new MessageMetadata(sourceCorrelationId, messageDateTime);

        public override string ToString()
        {
            return $"MessageMetadata: {SourceCorrelationId} @ {MessageDateTime}";
        }

        protected bool Equals(MessageMetadata other)
        {
            return string.Equals(SourceCorrelationId, other.SourceCorrelationId) 
                   && MessageDateTime.Equals(other.MessageDateTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MessageMetadata)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SourceCorrelationId != null ? SourceCorrelationId.GetHashCode() : 0) * 397) ^ MessageDateTime.GetHashCode();
            }
        }

        public static bool operator ==(MessageMetadata left, MessageMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MessageMetadata left, MessageMetadata right)
        {
            return !Equals(left, right);
        }
    }
}
