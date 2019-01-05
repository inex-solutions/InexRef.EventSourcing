using System.Threading.Tasks;
using InexRef.EventSourcing.Persistence.Common;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Shouldly;

namespace InexRef.EventSourcing.Persistence.Tests
{
    public class when_get_is_called_for_a_non_existent_aggregate : AggregateRepositoryTestBase
    {
        public when_get_is_called_for_a_non_existent_aggregate(string hostingFlavour) : base(hostingFlavour) { }

        protected override async Task When() => CaughtException = await Catch.AsyncException(() => Subject.Get(AggregateId));

        [Then]
        public void an_aggregate_not_found_exception_is_thrown() => CaughtException.ShouldBeOfType<AggregateNotFoundException>();
    }
}