using System;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Persistence.SqlServer.Persistence;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using NUnit.Framework;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.SqlServer.Tests
{
    [TestFixture]
    public class SqlEventStoreJsonSerializerTests : SpecificationBase<SqlEventStoreJsonSerializer>
    {
        private IEvent<Guid> SourceEvent { get; set; }
        private IEvent RehydratedEvent { get; set; }

        protected override void Given()
        {
            Subject = new SqlEventStoreJsonSerializer();
            SourceEvent = new TestEvent(Guid.NewGuid(), MessageMetadata.CreateDefault());
        }

        protected override void When()
        {
            var json = Subject.Serialize(SourceEvent);
            RehydratedEvent = Subject.Deserialize(json, SourceEvent.MessageMetadata.SourceCorrelationId, SourceEvent.MessageMetadata.MessageDateTime);
        }

        [Then]
        public void the_rehydrated_event_is_equal_to_the_source_event() => SourceEvent.ShouldBe(RehydratedEvent);

        public void DirectlyReferenceNUnitToAidTestRunner()
        {
            Assert.IsTrue(true);
        }
    }
}
