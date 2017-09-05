using Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.AggregateTests;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.EventSourcing.Tests.AggregateTests
{
    public class when_I_add_an_one_pound_to_an_empty_account : AggregateRootTestBase<AccountAggregateRoot>
    {
        protected override void Given() { }

        protected override void When() => Subject.AddAmount(1.00M);

        [Then]
        public void the_balance_is_one_pound() => Subject.Balance.ShouldBe(1.00M);
    }
}