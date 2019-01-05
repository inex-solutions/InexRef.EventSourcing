using System;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class CounterInitialisedEvent : Event<Guid>
    {
        public CounterInitialisedEvent(MessageMetadata messageMetadata, Guid id) : base(messageMetadata, id)
        {
        }
    }
}