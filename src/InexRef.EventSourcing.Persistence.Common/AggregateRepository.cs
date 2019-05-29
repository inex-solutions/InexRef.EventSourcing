#region Copyright & Licence
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

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
