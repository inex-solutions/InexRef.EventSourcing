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

using System.Collections.Generic;
using Autofac;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Tests
{
    public class when_sending_an_increment_command_to_a_counter_with_a_current_value_of_one : CounterTestBase
    {
        public when_sending_an_increment_command_to_a_counter_with_a_current_value_of_one(string testFixtureOptions) : base(testFixtureOptions) { }

        private readonly List<CounterValueDivisibleByTwoEvent> _counterValueDivisibleByTwoEvents = new List<CounterValueDivisibleByTwoEvent>();

        private MessageMetadata MetadataOnFinalIncrementCommand { get; set; }


        protected override void RegisterWithContainerBuilder(ContainerBuilder containerBuilder)
        {
            base.RegisterWithContainerBuilder(containerBuilder);
            var handler = new CounterValueDivisibleByTwoEventHandler(evt => _counterValueDivisibleByTwoEvents.Add(evt));

            containerBuilder.RegisterInstance(handler).As<IHandle<CounterValueDivisibleByTwoEvent>>();
        }

        protected override void Given()
        {
            Subject.Send(new InitialiseCounterCommand(MessageMetadata.CreateDefault(), NaturalId));
            Subject.Send(new IncrementCounterCommand(MessageMetadata.CreateDefault(), NaturalId));

            MetadataOnFinalIncrementCommand = MessageMetadata.CreateDefault();
        }

        protected override void When() => Subject.Send(new IncrementCounterCommand(MetadataOnFinalIncrementCommand, NaturalId));

        [Then]
        public void the_counter_value_is_two()
            => Repository.GetByNaturalKey(NaturalId).CurrentValue.ShouldBe(2);

        [Then]
        public void a_single_divisible_by_two_event_was_sent()
            => _counterValueDivisibleByTwoEvents.ShouldHaveACountOf(1);

        [Then]
        public void the_current_value_on_the_event_is_two()
            => _counterValueDivisibleByTwoEvents[0].CurrentValue.ShouldBe(2);

        [Then]
        public void the_message_metadata_source_correlation_id_on_the_divisible_by_two_event_matches_the_source_command()
            => _counterValueDivisibleByTwoEvents[0].MessageMetadata.SourceCorrelationId.ShouldBe(MetadataOnFinalIncrementCommand.SourceCorrelationId);

        [Then]
        public void the_message_metadata_event_date_on_the_divisible_by_two_event_is_later_than_the_the_source_command()
            => _counterValueDivisibleByTwoEvents[0].MessageMetadata.MessageDateTime.ShouldBeGreaterThan(MetadataOnFinalIncrementCommand.MessageDateTime);
    };
}