using System;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class CounterIncrementedEvent : Event<Guid>
    {
        public CounterIncrementedEvent(MessageMetadata messageMetadata, Guid id) : base(messageMetadata, id)
        {
        }
    }
}