using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Persistence.Common;
using Newtonsoft.Json;

namespace InexRef.EventSourcing.Persistence.EventStore
{
    public class EventStore<TId> : IEventStore<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        private readonly EventStoreConfiguration _eventStoreConfiguration;

        public EventStore(EventStoreConfiguration eventStoreConfiguration)
        {
            _eventStoreConfiguration = eventStoreConfiguration;
        }

        public IEnumerable<IEvent<TId>> LoadEvents(TId aggregateId, Type aggregateType, bool throwIfNotFound)
        {
            var connection = EventStoreConnection.Create(_eventStoreConfiguration.TcpConnectionString);
            var streamName = AggregateRootUtils.GetAggregateRootName(aggregateType) + "-" + aggregateId;

            connection.ConnectAsync().Wait();

            var streamEvents =
                connection.ReadStreamEventsForwardAsync(streamName, 0, 4095, false)
                    .Result; //TODO: RJL: remove the 4095 limit

            if (!streamEvents.Events.Any())
            {
                if (throwIfNotFound)
                {
                    throw new AggregateNotFoundException($"Aggregate '{aggregateId}' not found");
                }

                return Enumerable.Empty<IEvent<TId>>();
            }

            return streamEvents.Events.Select(e => (IEvent<TId>) FromJsonBytes(e.Event.Data));
        }

        public async Task SaveEvents(TId id, Type aggregateType, IEnumerable<IEvent<TId>> events, int currentVersion,
            int expectedVersion)
        {
            var connection = EventStoreConnection.Create(_eventStoreConfiguration.TcpConnectionString);
            var streamName = AggregateRootUtils.GetAggregateRootName(aggregateType) + "-" + id;

            await connection.ConnectAsync();
            var eventData = events.Select(e =>
                new EventData(Guid.NewGuid(), e.GetType().Name, true, ToJsonBytes(e), null));
            await connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventData);
        }

        public async Task DeleteEvents(TId id, Type aggregateType)
        {
            var streamName = AggregateRootUtils.GetAggregateRootName(aggregateType) + "-" + id;

            var urlBase = $"http://{_eventStoreConfiguration.Host}:{_eventStoreConfiguration.HttpPort}";

            var httpClient = new HttpClient();
            var message = new HttpRequestMessage(HttpMethod.Delete, $"{urlBase}/streams/{streamName}");
            message.Headers.Add("ES-HardDelete", "true");
            var response = await httpClient.SendAsync(message);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new EventStoreDeletionFailedException($"Deletion failed for id '{id}'. HttpStatusCode={response.StatusCode}, Body={await response.Content.ReadAsStringAsync()}");
            }
        }

        private byte[] ToJsonBytes(object source)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };

            string json = JsonConvert.SerializeObject(source, Formatting.Indented, jsonSettings);
            return new UTF8Encoding(false).GetBytes(json);
        }

        private object FromJsonBytes(byte[] bytes)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };

            var eventString = new UTF8Encoding(false).GetString(bytes);
            var @event = JsonConvert.DeserializeObject(eventString, jsonSettings);
            return @event;
        }
    }
}
