using System;
using System.Linq;
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.Common;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    [TestFixtureSource(typeof(NUnitTestFixtureSource), "TestFlavours")]
    public class when_an_event_is_saved_and_reloaded : SpecificationBase
    {
        private readonly string _testFlavour;
        private Event<Guid> _eventToSave;
        private MessageMetadata _messageMetadata;
        private Guid _id;
        private IEventStore<Guid> EventStore { get; set; }
        private IEvent<Guid> ReloadedEvent { get; set; }

        public when_an_event_is_saved_and_reloaded(string testFlavour)
        {
            _testFlavour = testFlavour;
        }

        protected override void SetUp()
        {
            var containerBuilder = new ServiceCollection();
            TestEnvironmentSetup.ConfigureContainerForHostEnvironmentFlavour(containerBuilder, _testFlavour);
            containerBuilder.RegisterModule<EventSourcingCoreModule>();

            var container = containerBuilder.BuildServiceProvider();

            EventStore = container.GetService<IEventStore<Guid>>();
        }

        protected override void Given()
        {
            _id = Guid.NewGuid();
            _messageMetadata = MessageMetadata.CreateDefault();
            _eventToSave = new CounterIncrementedEvent(_messageMetadata, _id);

            EventStore.SaveEvents(_id, typeof(CounterAggregateRoot), new [] {_eventToSave }, 1, 0).GetAwaiter().GetResult();
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