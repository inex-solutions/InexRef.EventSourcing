using System;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Persistence.SqlServer.Persistence
{
    public interface ISqlEventStoreJsonSerializer
    {
        string Serialize(IEvent @event);

        IEvent Deserialize(string eventPayload, string sourceCorrelationId, DateTime eventDateTime);
    }
}