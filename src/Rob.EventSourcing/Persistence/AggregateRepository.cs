﻿#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017 INEX Solutions Ltd
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
using Rob.EventSourcing.Contracts;
using Rob.EventSourcing.Contracts.Bus;
using Rob.EventSourcing.Contracts.Messages;
using Rob.EventSourcing.Contracts.Persistence;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public class AggregateRepository<TAggregate, TId> : IAggregateRepository<TAggregate, TId>
        where TAggregate : IAggregateRoot<TId>, IAggregateRootInternal<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly IAggregateRootFactory _aggregateRootFactory;
        private readonly IEventStore<TId> _eventStore;
        private readonly IBus _bus;

        public AggregateRepository(
            IAggregateRootFactory aggregateRootFactory,
            IEventStore<TId> eventStore, 
            IBus bus)
        {
            _aggregateRootFactory = aggregateRootFactory;
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

            foreach (var eventToStore in events)
            {
                _bus.PublishEvent(eventToStore);
            }

            var eventsToPublish = internalAggregate.GetUnpublishedEvents();
            foreach (IEventInternal @event in eventsToPublish)
            {
                @event.SetVersion(version);
                _bus.PublishEvent((IEvent<TId>)@event);
            }

            _eventStore.SaveEvents(aggregate.Id, typeof(TAggregate), events, version, aggregate.Version);
            internalAggregate.Dispose();
        }

        public TAggregate Get(TId id)
        {
            IAggregateRootInternal<TId> aggregate = _aggregateRootFactory.Create<TAggregate>();
            var events = _eventStore.LoadEvents(id, typeof(TAggregate), throwIfNotFound: true).ToList();
            aggregate.Load(id, events);
            return (TAggregate)aggregate;
        }

        public TAggregate GetOrCreateNew(TId id, Action<TAggregate> onCreateNew)
        {
            var aggregate = _aggregateRootFactory.Create<TAggregate>();
            IList<IEvent<TId>> events = _eventStore.LoadEvents(id, typeof(TAggregate), throwIfNotFound: false).ToList();

            aggregate.Load(id, events);

            if (!events.Any())
            {
                onCreateNew?.Invoke(aggregate);
            }

            return aggregate;
        }

        public void Delete(TId id)
        {
            _eventStore.DeleteEvents(id, typeof(TAggregate));
        }
    }
}
