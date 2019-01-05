using System;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Persistence.SqlServer.Tests
{
    public class TestEvent : Event<Guid>
    {
        public TestEvent(Guid id, MessageMetadata messageMetadata) : base(messageMetadata, id)
        {
        }
    }
}