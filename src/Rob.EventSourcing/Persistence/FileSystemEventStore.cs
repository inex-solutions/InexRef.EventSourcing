using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public class FileSystemEventStore : IEventStore
    {
        private readonly DirectoryInfo _directory;
        private readonly JsonSerializer _jsonSerializer;

        public FileSystemEventStore(FilePersistenceConfiguration filePersistenceConfiguration)
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

        public IEnumerable<Event> LoadEvents(Guid aggregateId)
        {
            IEnumerable<Event> events;
            if (!TryLoadEvents(aggregateId, out events))
            {
                throw new AggregateNotFoundException($"Aggregate '{aggregateId}' not found");
            }
            return events;
        }

        bool IEventStore.TryLoadEvents(Guid aggregateId, out IEnumerable<Event> events)
        {
            return TryLoadEvents(aggregateId, out events);
        }

        void IEventStore.SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            SaveEvents(aggregateId, events, expectedVersion);
        }

        public bool TryLoadEvents(Guid aggregateId, out IEnumerable<Event> events)
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
                var persisted = (PersistedAggregate)_jsonSerializer.Deserialize(reader, typeof(PersistedAggregate));
                events = persisted.Events;
                return true;
            }
        }

        public void SaveEvents(Guid id, Type aggregateType, IEnumerable<Event> events, int expectedVersion)
        {
            var eventsList = events.ToList();

            int eventVersion = expectedVersion;

            foreach (var @event in eventsList)
            {
                @event.Version = ++eventVersion;
            }

            var persistedAggregate = new PersistedAggregate
            {
                Metadata = new Metadata
                {
                    Id = id,
                    AggregateType = aggregateType.FullName,
                    Version = eventVersion
                },

                Events = eventsList
            };

            using (var fileStream = new FileStream(Path.Combine(_directory.FullName, id + ".json"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                if (fileStream.Length > 0)
                {
                    var reader = new StreamReader(fileStream, Encoding.UTF8);
                    var persisted = (PersistedAggregate) _jsonSerializer.Deserialize(reader, typeof(PersistedAggregate));
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

        public void DeleteEvents(Guid id, Type aggregateType)
        {
            File.Delete(Path.Combine(_directory.FullName, id + ".json"));
        }

        public class PersistedAggregate
        {
            public Metadata Metadata { get; set; }

            public IEnumerable<Event> Events { get; set; }
        }

        public class Metadata
        {
            public Guid Id {get; set; }

            public string AggregateType { get; set; }

            public int Version { get; set; }
        }

        IEnumerable<Event> IEventStore.LoadEvents(Guid aggregateId)
        {
            return LoadEvents(aggregateId);
        }
    }
}