using System;
using System.Collections.Generic;
using ReflectionMagic;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow
{
    public abstract class AggregateRoot : IAggregateRootInternal
    {
        private readonly List<Event> _uncommittedEvents = new List<Event>();

        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        public IEnumerable<Event> GetUncommittedEvents()
        {
            return _uncommittedEvents;
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