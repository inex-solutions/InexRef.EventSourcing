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
using System.Collections.Generic;
using System.Linq;
using Autofac;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.Common;
using InexRef.EventSourcing.Tests.Common.Persistence;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using NUnit.Framework;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    [TestFixture("EventStorePersistence=InMemory", Category = "DomainOnly")]
    [TestFixture("EventStorePersistence=SqlServer", Category = "DomainHosting")]
    public class when_an_event_is_saved_and_reloaded : SpecificationBase
    {
        private Event<Guid> _eventToSave;
        private MessageMetadata _messageMetadata;
        private Guid _id;
        private readonly Dictionary<string, string> _testFixtureOptions;
        private IEventStore<Guid> EventStore { get; set; }
        private IEvent<Guid> ReloadedEvent { get; set; }

        public when_an_event_is_saved_and_reloaded(string testFixtureOptions)
        {
            _testFixtureOptions = testFixtureOptions
                .Split(',')
                .ToDictionary(item => item.Split('=')[0].Trim(), item => item.Split('=')[1].Trim());
        }

        protected override void SetUp()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<EventSourcingCoreModule>();
            containerBuilder.RegisterEventStorePersistenceModule(_testFixtureOptions["EventStorePersistence"]);
            containerBuilder.RegisterModule<TestSetupModule>();

            var container = containerBuilder.Build();

            EventStore = container.Resolve<IEventStore<Guid>>();
        }

        protected override void Given()
        {
            _id = Guid.NewGuid();
            _messageMetadata = MessageMetadata.CreateDefault();
            _eventToSave = new CounterIncrementedEvent(_messageMetadata, _id);

            EventStore.SaveEvents(_id, typeof(CounterAggregateRoot), new [] {_eventToSave }, 1, 0);
        }

        protected override void When() 
            => ReloadedEvent = EventStore.LoadEvents(_id, typeof(CounterAggregateRoot), true).FirstOrDefault();

        [Then]
        public void the_reloaded_event_metadata_has_the_correct_correlation_id()
            => ReloadedEvent.MessageMetadata.SourceCorrelationId.ShouldBe(_messageMetadata.SourceCorrelationId);

        [Then]
        public void the_reloaded_event_metadata_is_the_same_as_the_metatdata_on_the_saved_event()
            => ReloadedEvent.MessageMetadata.ShouldBe(_messageMetadata);
    }
}