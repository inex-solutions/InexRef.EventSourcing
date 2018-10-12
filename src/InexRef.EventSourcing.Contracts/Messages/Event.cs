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