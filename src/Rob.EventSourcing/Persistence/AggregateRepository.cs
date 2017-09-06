using System;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public class AggregateRepository<TAggregate, TId> : IAggregateRepository<TAggregate, TId>
        where TAggregate : AggregateRoot<TId>, new()
        where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly IEventStore<TId> _eventStore;
        private readonly IBus _bus;

        public AggregateRepository(IEventStore<TId> eventStore, IBus bus)
        {
            _eventStore = eventStore;
            _bus = bus;
        }

        public void Save(TAggregate aggregate)
        {
            IAggregateRootInternal<TId> internalAggregate = aggregate;

            var events = internalAggregate.GetUncommittedEvents().ToList();

            int version = aggregate.Version;

            foreach (IEventInternal @event in events)
            {
                @event.SetVersion(++version);
            }

            foreach (IEventInternal @event in internalAggregate.GetUnpublishedEvents())
            {
                @event.SetVersion(version);
                _bus.PublishEvent((Event)@event);
            }

            _eventStore.SaveEvents(aggregate.Id, typeof(TAggregate), events, version, aggregate.Version);
            internalAggregate.ClearUncommittedEvents();
            internalAggregate.ClearUnpublishedEvents();
            internalAggregate.Dispose();
        }

        public TAggregate Get(TId id)
        {
            IAggregateRootInternal<TId> aggregate = new TAggregate();
            var events = _eventStore.LoadEvents(id).ToList();
            aggregate.Load(id, events);
            return (TAggregate)aggregate;
        }

        public TAggregate GetOrCreateNew(TId id)
        {
            IAggregateRootInternal<TId> aggregate = new TAggregate();
            IEnumerable<Event> events;
            if (!_eventStore.TryLoadEvents(id, out events))
            {
                events = new List<Event>();
            }
            aggregate.Load(id, events.ToList());
            return (TAggregate)aggregate;
        }

        public void Delete(TId id)
        {
            _eventStore.DeleteEvents(id, typeof(TAggregate));
        }
    }
}
