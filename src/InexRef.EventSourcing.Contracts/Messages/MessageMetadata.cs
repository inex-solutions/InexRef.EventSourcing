#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2018 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

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
