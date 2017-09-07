using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class when_two_pounds_is_added_to_a_non_existent_account : IntegrationTestBase
    {
        public when_two_pounds_is_added_to_a_non_existent_account(string persistenceProvider) : base(persistenceProvider) { }

        protected override void Given() { }

        protected override void When() => Subject.Send(new AddAmountCommand(AggregateId, 2.00M));

        [Then]
        public void a_new_account_is_created_with_a_balance_of_two_pounds() => Repository.Get(AggregateId).Balance.ShouldBe(2.00M);

        [Then]
        public void the_account_balance_on_the_read_model_is_two_pounds() => BalanceReadModel[AggregateId].ShouldBe(2.00M);

        [Then]
        public void the_aggregate_is_version_one() => Repository.Get(AggregateId).Version.ShouldBe(1);

        [Then]
        public void the_version_on_the_read_model_is_one() => BalanceReadModel.GetVersion(AggregateId).ShouldBe(1);

        [Then]
        public void the_total_number_of_updates_received_by_a_subscribed_read_model_is_one() => ReceivedEventsHistoryReadModel[AggregateId].Count.ShouldBe(1);
    }
}