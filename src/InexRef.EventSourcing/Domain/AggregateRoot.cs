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
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Utils.Dynamic;

namespace InexRef.EventSourcing.Domain
{
    public abstract class AggregateRoot<TId> : IAggregateRoot<TId>, IAggregateRootInternal<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private bool _isDisposed;

        private readonly List<IEvent<TId>> _uncommittedEvents = new List<IEvent<TId>>();

        public TId Id { get; protected set; }

        public int Version { get; protected set; }

        public abstract string Name { get; }

        protected void Apply(IEvent<TId> @event)
        {
            ThrowIfDisposed();
            Apply(@event, true);
        }

        private void HandleEvent(IEvent<TId> @event, bool isNew)
        {
            ThrowIfDisposed();
            this.DynamicallyInvokeMethod(OnMissingMethod.Ignore()).HandleEvent(@event, isNew);
        }

        protected void Apply(IEvent<TId> @event, bool isNew)
        {
            ThrowIfDisposed();
            HandleEvent(@event, isNew);

            if (@event.Version > Version)
            {
                Version = @event.Version;
            }

            if (isNew)
            {
                _uncommittedEvents.Add(@event);
            }
        }

        void IAggregateRootInternal<TId>.Load(TId id, IEnumerable<IEvent<TId>> eventHistory)
        {
            ThrowIfDisposed();
            Id = id;
            foreach (var @event in eventHistory)
            {
                Apply(@event, false);
            }
        }

        IEnumerable<IEvent<TId>> IAggregateRootInternal<TId>.GetUncommittedEvents()
        {
            ThrowIfDisposed();
            return _uncommittedEvents;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isDisposed = true;
                _uncommittedEvents.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name, "Aggregate has been disposed and cannot be used further (note: aggregate is automatically disposed on saving)");
            }
        }
    }
}