using System.Threading.Tasks;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    public class when_a_counter_with_a_count_of_one_is_saved_and_reloaded : AggregateRepositoryTestBase
    {
        public when_a_counter_with_a_count_of_one_is_saved_and_reloaded(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            var aggregate = AggregateRootFactory.Create<CounterAggregateRoot>();
            await aggregate.Initialise(AggregateId);
            await aggregate.Increment();
            await Subject.Save(aggregate);
        }

        protected override async Task When() => ReloadedCounterAggregateRoot = await Subject.Get(AggregateId);

        [Then]
        public void the_reloaded_counter_should_have_a_value_of_one() => ReloadedCounterAggregateRoot.CurrentValue.ShouldBe(1);
    }
}