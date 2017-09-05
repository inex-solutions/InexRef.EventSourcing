using Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.EventSourcing.Tests.IntegrationTests
{
    public class when_two_pounds_is_added_to_an_existing_but_empty_account : IntegrationTestBase
    {
        protected override void Given()
        {
            var account = new AccountAggregateRoot(AggregateId);
            Repository.Save(account, account.Version);
        }

        protected override void When() => Subject.Send(new AddAmountCommand(AggregateId, 2.00M));

        [Then]
        public void the_account_balance_is_two_pounds() => Repository.Get(AggregateId).Balance.ShouldBe(2.00M);
    }
}