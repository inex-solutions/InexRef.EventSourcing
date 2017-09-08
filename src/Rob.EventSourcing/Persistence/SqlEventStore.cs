using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Rob.EventSourcing.Contracts.Messages;
using Rob.EventSourcing.Contracts.Persistence;

namespace Rob.EventSourcing.Persistence
{
    public class SqlEventStore<TId> : IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly DirectoryInfo _directory;
        private readonly JsonSerializer _jsonSerializer;

        public SqlEventStore(FilePersistenceConfiguration filePersistenceConfiguration)
        {
            _directory = new DirectoryInfo(filePersistenceConfiguration.EventStoreRootDirectory);
            if (!_directory.Exists)
            {
                _directory.Create();
                _directory.Refresh();
            }

            _jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
        }

        public IEnumerable<IEvent<TId>> LoadEvents(TId aggregateId)
        {
            IEnumerable<IEvent<TId>> events;
            if (!TryLoadEvents(aggregateId, out events))
            {
                throw new AggregateNotFoundException($"Aggregate '{aggregateId}' not found");
            }
            return events;
        }

        public bool TryLoadEvents(TId aggregateId, out IEnumerable<IEvent<TId>> events)
        {
            var filename = Path.Combine(_directory.FullName, aggregateId + ".json");
            if (!File.Exists(filename))
            {
                events = null;
                return false;
            }
            using (var fileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(fileStream, Encoding.UTF8))
            {
                var persisted = (PersistedAggregate<TId>)_jsonSerializer.Deserialize(reader, typeof(PersistedAggregate<TId>));
                events = persisted.Events;
                return true;
            }
        }

        public void SaveEvents(TId id, Type aggregateType, IEnumerable<IEvent<TId>> events, int currentVersion, int expectedVersion)
        {
            var persistedAggregate = new PersistedAggregate<TId>
            {
                Metadata = new Metadata<TId>
                {
                    Id = id,
                    AggregateType = aggregateType.FullName,
                    Version = currentVersion
                },

                Events = events
            };

            using (var fileStream = new FileStream(Path.Combine(_directory.FullName, id + ".json"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                if (fileStream.Length > 0)
                {
                    var reader = new StreamReader(fileStream, Encoding.UTF8);
                    var persisted = (PersistedAggregate<TId>)_jsonSerializer.Deserialize(reader, typeof(PersistedAggregate<TId>));
                    persistedAggregate.Events = persisted.Events.Concat(persistedAggregate.Events);
                    fileStream.Position = 0;

                    if (persisted.Metadata.Version != expectedVersion)
                    {
                        throw new EventStoreConcurrencyException($"Concurrency error saving aggregate {id} (expected version {expectedVersion}, actual {persisted.Metadata.Version})");
                    }
                }

                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    _jsonSerializer.Serialize(writer, persistedAggregate);
                }
            }
        }

        public void DeleteEvents(TId id, Type aggregateType)
        {
            File.Delete(Path.Combine(_directory.FullName, id + ".json"));
        }
    }
}