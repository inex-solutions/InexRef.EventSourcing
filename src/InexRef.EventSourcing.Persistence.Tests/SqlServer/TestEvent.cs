using System;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Persistence.Tests.SqlServer
{
    public class TestEvent : Event<Guid>
    {
        public TestEvent(Guid id, MessageMetadata messageMetadata) : base(messageMetadata, id)
        {
        }
    }
}