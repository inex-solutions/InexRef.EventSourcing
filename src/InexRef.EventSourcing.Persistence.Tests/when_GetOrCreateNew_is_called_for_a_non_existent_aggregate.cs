using System.Threading.Tasks;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    public class when_GetOrCreateNew_is_called_for_a_non_existent_aggregate : AggregateRepositoryTestBase
    {
        public when_GetOrCreateNew_is_called_for_a_non_existent_aggregate(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task When() => ReloadedCounterAggregateRoot = await Subject.GetOrCreateNew(AggregateId, null);

        [Then]
        public void no_exception_is_thrown_and_a_new_aggregate_is_returned() => ReloadedCounterAggregateRoot
            .ShouldNotBeNull();
    }
}