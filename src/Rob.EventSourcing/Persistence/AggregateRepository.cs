using System;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public class AggregateRepository<TAggregate> : IAggregateRepository<TAggregate>
        where TAggregate : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;
        private readonly IBus _bus;

        public AggregateRepository(IEventStore eventStore, IBus bus)
        {
            _eventStore = eventStore;
            _bus = bus;
        }

        public void Save(TAggregate aggregate)
        {
            IAggregateRootInternal internalAggregate = aggregate;

            var events = internalAggregate.GetUncommittedEvents().ToList();

            int version = aggregate.Version;

            foreach (var @event in events)
            {
                @event.Version = ++version;
            }

            foreach (var @event in internalAggregate.GetUnpublishedEvents())
            {
                @event.Version = version;
                _bus.PublishEvent(@event);
            }

            _eventStore.SaveEvents(aggregate.Id, typeof(TAggregate), events, version, aggregate.Version);
            internalAggregate.ClearUncommittedEvents();
            internalAggregate.ClearUnpublishedEvents();
            internalAggregate.Dispose();
        }

        public TAggregate Get(Guid id)
        {
            IAggregateRootInternal aggregate = new TAggregate();
            var events = _eventStore.LoadEvents(id).ToList();
            aggregate.Load(id, events);
            return (TAggregate)aggregate;
        }

        public TAggregate GetOrCreateNew(Guid id)
        {
            IAggregateRootInternal aggregate = new TAggregate();
            IEnumerable<Event> events;
            if (!_eventStore.TryLoadEvents(id, out events))
            {
                events = new List<Event>();
            }
            aggregate.Load(id, events.ToList());
            return (TAggregate)aggregate;
        }

        public void Delete(Guid id)
        {
            _eventStore.DeleteEvents(id, typeof(TAggregate));
        }
    }
}
