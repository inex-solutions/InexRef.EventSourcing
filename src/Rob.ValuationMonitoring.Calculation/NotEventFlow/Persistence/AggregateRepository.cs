using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public TAggregate GetById(Guid id)
        {
            IAggregateRootInternal aggregate = new TAggregate();
            var events = _eventStore.LoadEvents(id).ToList();
            aggregate.Load(id, events);
            return (TAggregate)aggregate;
        }
    }
}
