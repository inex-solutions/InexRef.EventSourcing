
using System;
using System.Collections.Generic;
using ReflectionMagic;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing
{
    public abstract class AggregateRoot : IAggregateRootInternal
    {
        private readonly List<Event> _uncommittedEvents = new List<Event>();
        private readonly List<Event> _eventsToPublish = new List<Event>();

        public Guid Id { get; protected set; }

        public  int Version { get; protected set; }

        public IEnumerable<Event> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public IEnumerable<Event> GetUnpublishedEvents()
        {
            return _eventsToPublish;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public void ClearUnpublishedEvents()
        {
            _eventsToPublish.Clear();
        }

        protected void PublishEvent(Event @event)
        {
            _eventsToPublish.Add(@event);
        }

        protected void Apply(Event @event)
        {
            Apply(@event, true);
        }

        private void Apply(Event @event, bool isNew)
        {
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

        void IAggregateRootInternal.Load(Guid id, IEnumerable<Event> eventHistory)
        {
            Id = id;
            foreach (var @event in eventHistory)
            {
                Apply(@event, false);
            }
        }
    }
}