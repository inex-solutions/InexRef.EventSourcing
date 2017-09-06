
using System;
using System.Collections.Generic;
using ReflectionMagic;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing
{
    public abstract class AggregateRoot<TId> : IAggregateRootInternal<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly List<Event> _uncommittedEvents = new List<Event>();
        private readonly List<Event> _eventsToPublish = new List<Event>();
        private bool _isDisposed;

        public TId Id { get; protected set; }

        public  int Version { get; protected set; }

        protected void PublishEvent(Event @event)
        {
            ThrowIfDisposed();
            _eventsToPublish.Add(@event);
        }

        protected void Apply(Event @event)
        {
            ThrowIfDisposed();
            Apply(@event, true);
        }

        private void Apply(Event @event, bool isNew)
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

        void IAggregateRootInternal<TId>.Load(TId id, IEnumerable<Event> eventHistory)
        {
            ThrowIfDisposed();
            Id = id;
            foreach (var @event in eventHistory)
            {
                Apply(@event, false);
            }
        }

        IEnumerable<Event> IAggregateRootInternal<TId>.GetUncommittedEvents()
        {
            ThrowIfDisposed();
            return _uncommittedEvents;
        }

        IEnumerable<Event> IAggregateRootInternal<TId>.GetUnpublishedEvents()
        {
            ThrowIfDisposed();
            return _eventsToPublish;
        }

        void IAggregateRootInternal<TId>.ClearUncommittedEvents()
        {
            ThrowIfDisposed();
            _uncommittedEvents.Clear();
        }

        void IAggregateRootInternal<TId>.ClearUnpublishedEvents()
        {
            ThrowIfDisposed();
            _eventsToPublish.Clear();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isDisposed = true;
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