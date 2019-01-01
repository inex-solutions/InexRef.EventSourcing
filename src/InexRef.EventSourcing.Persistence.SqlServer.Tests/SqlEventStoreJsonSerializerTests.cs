#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2018 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Persistence.SqlServer.Persistence;
using InexRef.EventSourcing.Persistence.Tests.SqlServer;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using NUnit.Framework;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.SqlServer.Tests
{
    [TestFixture(Category = "DomainOnly")]
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
