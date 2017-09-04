using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.IntegrationTests
{
    public class when_the_balance_is_reset_on_an_account_containing_two_pounds : IntegrationTestBase
    {
        protected override void Given()
        {
            var account = new AccountAggregateRoot(AggregateId);
            account.AddAmount(2.00M);
            Repository.Save(account, account.Version);
        }

        protected override void When() => Subject.Handle(new ResetBalanceCommand(AggregateId));

        [Then]
        public void the_account_balance_is_zero() => Repository.Get(AggregateId).Balance.ShouldBe(0.00M);
    }
}