using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence
{
    public class AggregateRepository<TAggregate> : IAggregateRepository<TAggregate>
        where TAggregate : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;

        public AggregateRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Save(TAggregate aggregate, int expectedVersion)
        {
            _eventStore.SaveEvents(aggregate.Id, aggregate.GetUncommittedEvents(), expectedVersion);
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
    }
}
