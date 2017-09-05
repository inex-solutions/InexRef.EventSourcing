using System;
using System.Collections.Generic;
using System.IO;
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
            var filename = Path.Combine(_directory.FullName, aggregateId.ToString() + ".json");
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

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            var persistedAggregate = new PersistedAggregate
            {
                Metadata = new Metadata
                {
                    Id = aggregateId,
                    AggregateType = "Unknown",
                    Version = -1
                },

                Events = events
            };

            using (var fileStream = new FileStream(Path.Combine(_directory.FullName, aggregateId.ToString()+".json"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
            {
                _jsonSerializer.Serialize(writer, persistedAggregate);
            }
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