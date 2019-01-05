using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.OperationContext;
using InexRef.EventSourcing.Contracts.Persistence;

namespace InexRef.EventSourcing.Persistence.Common
{
    public class AggregateRepository<TAggregate, TId> : IAggregateRepository<TAggregate, TId>
        where TAggregate : IAggregateRoot<TId>, IAggregateRootInternal<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly IAggregateRootFactory _aggregateRootFactory;
        private readonly IEventStore<TId> _eventStore;
        private readonly IBus _bus;
        private readonly IWriteableOperationContext _context;

        public AggregateRepository(
            IAggregateRootFactory aggregateRootFactory,
            IEventStore<TId> eventStore, 
            IBus bus,
            IWriteableOperationContext context)
        {
            _aggregateRootFactory = aggregateRootFactory;
            _eventStore = eventStore;
            _bus = bus;
            _context = context;
        }

        public async Task Save(TAggregate aggregate)
        {
            IAggregateRootInternal<TId> internalAggregate = aggregate;

            var events = internalAggregate.GetUncommittedEvents().ToList();

            int version = aggregate.Version;

            foreach (IEventInternal @event in events)
            {
                @event.SetVersion(++version);
            }

            foreach (var eventToStore in events)
            {
                await _bus.PublishEvent(eventToStore);
            }

            await _eventStore.SaveEvents(aggregate.Id, typeof(TAggregate), events, version, aggregate.Version);
            internalAggregate.Dispose();
        }

        public async Task<TAggregate> Get(TId id)
        {
            IAggregateRootInternal<TId> aggregate = _aggregateRootFactory.Create<TAggregate>();
            var events = _eventStore.LoadEvents(id, typeof(TAggregate), throwIfNotFound: true);

            _context.SetIsLoading(true);
            await aggregate.Load(id, events);
            _context.SetIsLoading(false);

            return (TAggregate)aggregate;
        }

        public async Task<TAggregate> GetOrCreateNew(TId id, Action<TAggregate> onCreateNew)
        {
            var aggregate = _aggregateRootFactory.Create<TAggregate>();
            IList<IEvent<TId>> events = (_eventStore.LoadEvents(id, typeof(TAggregate), throwIfNotFound: false)).ToList();

            _context.SetIsLoading(true);
            await aggregate.Load(id, events);
            _context.SetIsLoading(false);

            if (!events.Any())
            {
                onCreateNew?.Invoke(aggregate);
            }

            return aggregate;
        }

        public async Task Delete(TId id)
        {
            await _eventStore.DeleteEvents(id, typeof(TAggregate));
        }
    }
}
