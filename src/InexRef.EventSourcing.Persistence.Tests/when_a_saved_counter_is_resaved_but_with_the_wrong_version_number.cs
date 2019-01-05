using System.Threading.Tasks;
using InexRef.EventSourcing.Persistence.Common;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    public class when_a_saved_counter_is_resaved_but_with_the_wrong_version_number : AggregateRepositoryTestBase
    {
        public when_a_saved_counter_is_resaved_but_with_the_wrong_version_number(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            ReloadedCounterAggregateRoot = AggregateRootFactory.Create<NonDisposingCounterAggregateRoot>();
            await ReloadedCounterAggregateRoot.Initialise(AggregateId);

            await ReloadedCounterAggregateRoot.Increment();

            // intermediate save should cause a concurrency error when we save below
            await Subject.Save(ReloadedCounterAggregateRoot);

            await ReloadedCounterAggregateRoot.Increment();
        }

        protected override async Task When() => CaughtException = await Catch.AsyncException (async () => await Subject.Save(ReloadedCounterAggregateRoot));

        [Then]
        public void a_EventStoreConcurrencyException_should_be_thrown() => CaughtException.ShouldBeOfType<EventStoreConcurrencyException>();
    }
}