using System;
using System.Collections.Generic;
using ReflectionMagic;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow
{
    public abstract class AggregateRoot : IAggregateRootInternal
    {
        private readonly List<Event> _uncommittedEvents = new List<Event>();

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
        }

        void IAggregateRootInternal.Load(IEnumerable<Event> history)
        {
        }
    }
}