
using System;
using System.Collections.Generic;
using ReflectionMagic;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing
{
    public abstract class AggregateRoot<TId> : IAggregateRootInternal<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly List<IEvent<TId>> _uncommittedEvents = new List<IEvent<TId>>();
        private readonly List<IEvent<TId>> _eventsToPublish = new List<IEvent<TId>>();
        private bool _isDisposed;

        public TId Id { get; protected set; }

        public  int Version { get; protected set; }

        protected void PublishEvent(Event<TId> @event, bool isNew)
        {
            ThrowIfDisposed();

            if (isNew)
            {
                _eventsToPublish.Add(@event);
            }
        }

        protected void Apply(IEvent<TId> @event)
        {
            ThrowIfDisposed();
            Apply(@event, true);
        }

        private void Apply(IEvent<TId> @event, bool isNew)
        {
            ThrowIfDisposed();
            this.AsDynamic().HandleEvent(@event, isNew);

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

        IEnumerable<IEvent<TId>> IAggregateRootInternal<TId>.GetUnpublishedEvents()
        {
            ThrowIfDisposed();
            return _eventsToPublish;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isDisposed = true;
                _uncommittedEvents.Clear();
                _eventsToPublish.Clear();
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