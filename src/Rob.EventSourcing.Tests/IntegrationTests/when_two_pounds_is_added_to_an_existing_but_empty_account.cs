using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class when_two_pounds_is_added_to_an_existing_but_empty_account : IntegrationTestBase
    {
        protected override void Given()
        {
            var account = new AccountAggregateRoot(AggregateId);
            Repository.Save(account);
        }

        protected override void When() => Subject.Send(new AddAmountCommand(AggregateId, 2.00M));

        [Then]
        public void the_account_balance_is_two_pounds() => Repository.Get(AggregateId).Balance.ShouldBe(2.00M);

        [Then]
        public void the_account_balance_on_the_read_model_is_two_pounds() => BalanceReadModel[AggregateId].ShouldBe(2.00M);
    }

    public class when_two_pounds_is_added_to_a_new_account_followed_by_a_further_three_pounds : IntegrationTestBase
    {
        protected override void Given()
        {
            Subject.Send(new AddAmountCommand(AggregateId, 2.00M));
        }

        protected override void When() => Subject.Send(new AddAmountCommand(AggregateId, 3.00M));

        [Then]
        public void the_account_balance_is_five_pounds() => Repository.Get(AggregateId).Balance.ShouldBe(5.00M);

        [Then]
        public void the_aggregate_is_version_two() => Repository.Get(AggregateId).Version.ShouldBe(2);

        [Then]
        public void the_account_balance_on_the_read_model_is_five_pounds() => BalanceReadModel[AggregateId].ShouldBe(5.00M);

        [Then]
        public void the_version_on_the_read_model_is_two() => BalanceReadModel.GetVersion(AggregateId).ShouldBe(2);
    }
}