using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Tests
{
    public class when_sending_a_initialise_counter_command : CounterTestBase
    {
        public when_sending_a_initialise_counter_command(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task When() => await Subject.Send(new InitialiseCounterCommand(MessageMetadata.CreateDefault(), NaturalId));

        [Then]
        public async Task the_counter_is_created()
            => (await Repository.GetByNaturalKey(NaturalId)).ShouldNotBeNull();

        [Then]
        public async Task the_counter_value_is_zero()
            => (await Repository.GetByNaturalKey(NaturalId)).CurrentValue.ShouldBe(0);
    }
}