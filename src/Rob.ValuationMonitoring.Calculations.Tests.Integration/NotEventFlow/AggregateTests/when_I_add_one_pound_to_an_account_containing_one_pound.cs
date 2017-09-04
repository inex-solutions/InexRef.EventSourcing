using Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.PersistenceTests;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.AggregateTests
{
    public class when_I_add_one_pound_to_an_account_containing_one_pound : AggregateRootTestBase<AccountAggregateRoot>
    {
        protected override void Given() => Subject.AddAmount(1.00M);

        protected override void When() => Subject.AddAmount(1.00M);

        [Then]
        public void the_balance_is_two_pounds() => Subject.Balance.ShouldBe(2.00M);
    }
}