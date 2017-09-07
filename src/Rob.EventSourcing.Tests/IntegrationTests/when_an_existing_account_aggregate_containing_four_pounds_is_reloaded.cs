using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class when_an_existing_account_aggregate_containing_four_pounds_is_reloaded : IntegrationTestBase
    {
        private AccountAggregateRoot Aggregate { get; set; }

        public when_an_existing_account_aggregate_containing_four_pounds_is_reloaded(string persistenceProvider) : base(persistenceProvider) { }

        protected override void Given()
        {
            Subject.Send(new AddAmountCommand(AggregateId, 2.00M));
            Subject.Send(new AddAmountCommand(AggregateId, 2.00M));
            ReceivedEventsHistoryReadModel.Clear();
        }

        protected override void When() => Aggregate = Repository.Get(AggregateId);

        [Then]
        public void the_reloaded_aggregate_has_a_balance_of_four_pounds() => Aggregate.Balance.ShouldBe(4.0m);

        [Then]
        public void the_reloaded_aggregate_has_is_version_2() => Aggregate.Version.ShouldBe(2);

        [Then]
        public void no_updates_were_received_by_the_read_model() => ReceivedEventsHistoryReadModel[AggregateId].ShouldBeNull();
    }
}