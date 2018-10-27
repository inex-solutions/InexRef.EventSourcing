#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2018 INEX Solutions Ltd
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Persistence.Common;

namespace InexRef.EventSourcing.Persistence.InMemory
{
    public class InMemoryEventStore<TId> : IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly ConcurrentDictionary<TId, IEnumerable<StoredEvent<TId>>> _storedEvents = new ConcurrentDictionary<TId, IEnumerable<StoredEvent<TId>>>();

        public void SaveEvents(TId aggregateId, Type aggregateType, IEnumerable<IEvent<TId>> events, int currentVersion, int expectedVersion)
        {
            IEnumerable<StoredEvent<TId>> eventsToStore = events.Select(@event => new StoredEvent<TId>(@event.Version, @event)).ToList();

            _storedEvents.AddOrUpdate(
                key: aggregateId,
                addValue: eventsToStore,
                updateValueFactory: (id, enumerable) =>
                {
                    var persistedEvents = enumerable.ToList();
                    var version = persistedEvents.LastOrDefault()?.Version ?? 0;
                    if (version != expectedVersion)
                    {
                        throw new EventStoreConcurrencyException($"Concurrency error saving aggregate {id} (expected version {expectedVersion}, actual {version})");
                    }

                    var eventsToStoreList = eventsToStore.ToList();

                    return persistedEvents.Concat(eventsToStoreList);
                });
        }

        public void DeleteEvents(TId id, Type aggregateType)
        {
            _storedEvents.TryRemove(id, out IEnumerable<StoredEvent<TId>>  events);
        }

        public IEnumerable<IEvent<TId>> LoadEvents(TId aggregateId, Type aggregateType, bool throwIfNotFound)
        {
            IEnumerable<StoredEvent<TId>> events;

            if (!_storedEvents.TryGetValue(aggregateId, out events))
            {
                if (throwIfNotFound)
                {
                    throw new AggregateNotFoundException($"Aggregate '{aggregateId}' not found");
                }

                return Enumerable.Empty<IEvent<TId>>();
            }

            return events.Select(@event => @event.EventData);
        }

        private class StoredEvent<T> where T : IEquatable<T>, IComparable<T>
        {
            internal int Version { get; }

            internal IEvent<T> EventData { get; }

            internal StoredEvent(int version, IEvent<T> eventData)
            {
                Version = version;
                EventData = eventData;
            }
        }
    }
}