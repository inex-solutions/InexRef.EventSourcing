#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
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
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace InexRef.EventSourcing.Tests
{
    public class when_sending_an_increment_command_to_a_counter_with_a_current_value_of_one : CounterTestBase
    {
        public when_sending_an_increment_command_to_a_counter_with_a_current_value_of_one(string hostingFlavour) : base(hostingFlavour) { }

        private readonly List<CounterValueIsEvenEvent> _counterValueIsEventEvents = new List<CounterValueIsEvenEvent>();

        private MessageMetadata MetadataOnFinalIncrementCommand { get; set; }


        protected override void RegisterWithContainerBuilder(IServiceCollection containerBuilder)
        {
            base.RegisterWithContainerBuilder(containerBuilder);
            var handler = new CounterValueIsEvenEventHandler(evt =>
            {
                _counterValueIsEventEvents.Add(evt);
                return Task.CompletedTask;
            });

            containerBuilder.AddSingleton<IHandle<CounterValueIsEvenEvent>>(handler);
        }

        protected override async Task Given()
        {
            await Subject.Send(new InitialiseCounterCommand(MessageMetadata.CreateDefault(), NaturalId));
            await Subject.Send(new IncrementCounterCommand(MessageMetadata.CreateDefault(), NaturalId));

            MetadataOnFinalIncrementCommand = MessageMetadata.CreateDefault();
        }

        protected override async Task When() => await Subject.Send(new IncrementCounterCommand(MetadataOnFinalIncrementCommand, NaturalId));

        [Then]
        public async Task the_counter_value_is_two()
            => (await Repository.GetByNaturalKey(NaturalId)).CurrentValue.ShouldBe(2);

        [Then]
        public async Task a_single_counter_value_is_even_event_was_sent()
            => _counterValueIsEventEvents.ShouldHaveACountOf(1);

        [Then]
        public async Task the_current_value_on_the_event_is_two()
            => _counterValueIsEventEvents[0].CurrentValue.ShouldBe(2);

        [Then]
        public async Task the_message_metadata_source_correlation_id_on_the_counter_value_even_event_matches_the_source_command()
            => _counterValueIsEventEvents[0].MessageMetadata.SourceCorrelationId.ShouldBe(MetadataOnFinalIncrementCommand.SourceCorrelationId);

        [Then]
        public async Task the_message_metadata_event_date_on_the_counter_value_even_event_is_later_than_the_the_source_command()
            => _counterValueIsEventEvents[0].MessageMetadata.MessageDateTime.ShouldBeGreaterThan(MetadataOnFinalIncrementCommand.MessageDateTime);
    };

    public interface IFoo
    {
    }

    public interface IBar
    {
    }

    public class FooBar : IFoo, IBar
    {
    }

    [TestFixture]
    public class Scoping
    {
        [Test]
        public void WhenRegisteredAsForwardedSingleton_InstancesAreTheSame()
        {
            var services = new ServiceCollection();

            services.AddScoped<FooBar>(); // We must explicitly register Foo
            services.AddScoped<IFoo>(x => x.GetRequiredService<FooBar>()); // Forward requests to Foo
            services.AddScoped<IBar>(x => x.GetRequiredService<FooBar>()); // Forward requests to Foo

            IServiceProvider provider = services.BuildServiceProvider();

            provider = provider.CreateScope().ServiceProvider;

            var foo1 = provider.GetService<FooBar>(); // An instance of Foo
            var foo2 = provider.GetService<IFoo>(); // An instance of Foo
            var foo3 = provider.GetService<IBar>(); // An instance of Foo

            Assert.AreSame(foo1, foo2); // PASSES
            Assert.AreSame(foo1, foo3); // PASSES
        }
    }
}