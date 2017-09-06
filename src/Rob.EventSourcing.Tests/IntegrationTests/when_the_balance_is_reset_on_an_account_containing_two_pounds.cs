using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class when_the_balance_is_reset_on_an_account_containing_two_pounds : IntegrationTestBase
    {
        public when_the_balance_is_reset_on_an_account_containing_two_pounds(string persistenceProvider) : base(persistenceProvider) { }

        protected override void Given()
        {
            var account = new AccountAggregateRoot(AggregateId);
            account.AddAmount(2.00M);
            Repository.Save(account);
        }

        protected override void When() => Subject.Send(new ResetBalanceCommand(AggregateId));

        [Then]
        public void the_account_balance_is_zero() => Repository.Get(AggregateId).Balance.ShouldBe(0.00M);

        [Then]
        public void the_account_balance_on_the_read_model_is_zero() => BalanceReadModel[AggregateId].ShouldBe(0.00M);

        [Then]
        public void the_aggregate_is_version_two() => Repository.Get(AggregateId).Version.ShouldBe(2);

        [Then]
        public void the_version_on_the_read_model_is_two() => BalanceReadModel.GetVersion(AggregateId).ShouldBe(2);
    }
}