using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Tests
{
    public class when_sending_an_increment_command_to_a_newly_initialised_counter : CounterTestBase
    {
        public when_sending_an_increment_command_to_a_newly_initialised_counter(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            await Subject.Send(new InitialiseCounterCommand(MessageMetadata.CreateDefault(), NaturalId));
        }

        protected override async Task When() => await Subject.Send(new IncrementCounterCommand (MessageMetadata.CreateDefault(), NaturalId));

        [Then]
        public async Task the_counter_value_is_one()
            => (await Repository.GetByNaturalKey(NaturalId)).CurrentValue.ShouldBe(1);
    }
}