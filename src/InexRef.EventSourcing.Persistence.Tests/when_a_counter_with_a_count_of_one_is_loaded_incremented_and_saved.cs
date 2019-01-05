using System.Threading.Tasks;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    public class when_a_counter_with_a_count_of_one_is_loaded_incremented_and_saved : AggregateRepositoryTestBase
    {
        public when_a_counter_with_a_count_of_one_is_loaded_incremented_and_saved(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            var aggregate = AggregateRootFactory.Create<CounterAggregateRoot>();
            await aggregate.Initialise(AggregateId);
            await aggregate.Increment();
            await Subject.Save(aggregate);

            ReloadedCounterAggregateRoot = await Subject.Get(AggregateId);
            await ReloadedCounterAggregateRoot.Increment();
            await Subject.Save(ReloadedCounterAggregateRoot);
        }

        protected override async Task When() => ReloadedCounterAggregateRoot = await Subject.Get(AggregateId);

        [Then]
        public void then_the_counter_has_a_value_of_two_when_reloaded() => ReloadedCounterAggregateRoot.CurrentValue.ShouldBe(2);
    }
}