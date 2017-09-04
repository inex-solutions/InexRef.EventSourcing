using System;
using System.Collections.Generic;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow
{
    public abstract class AggregateRoot : IAggregateRootInternal
    {
        private readonly List<Event> _uncommittedEvents = new List<Event>();

        public abstract Guid Id { get; }

        public int Version { get; internal set; }

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

        }

        void IAggregateRootInternal.Load(IEnumerable<Event> history)
        {
        }
    }
}