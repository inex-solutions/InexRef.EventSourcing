using Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.AggregateTests;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.EventSourcing.Tests.AggregateTests
{
    public class when_I_reset_an_account_containing_one_pound : AggregateRootTestBase<AccountAggregateRoot>
    {
        protected override void Given() => Subject.AddAmount(1.00M);

        protected override void When() => Subject.ResetBalance();

        [Then]
        public void the_balance_is_zero() => Subject.Balance.ShouldBe(0.00M);
    }
}