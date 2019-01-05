using System;
using System.Threading.Tasks;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using InexRef.EventSourcing.Tests.Domain;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    public class when_an_aggregate_is_saved_but_then_subsequently_accessed : AggregateRepositoryTestBase
    {
        public when_an_aggregate_is_saved_but_then_subsequently_accessed(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task Given()
        {
            Aggregate = AggregateRootFactory.Create<CounterAggregateRoot>();
            await Aggregate.Initialise(AggregateId);
            await Subject.Save(Aggregate);
        }

        public CounterAggregateRoot Aggregate { get; set; }

        protected override async Task When() => CaughtException = await Catch.AsyncException(() => Aggregate.Increment());

        [Then]
        public void an_ObjectDisposedException_is_thrown() => CaughtException.ShouldBeOfType<ObjectDisposedException>();
    }
}