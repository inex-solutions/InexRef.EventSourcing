using Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.AggregateTests;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.PersistenceTests
{
    public class when_a_saved_account_containing_two_pounds_is_loaded_has_five_pounds_added_and_saved : AggregateRepositoryTestBase
    {
        protected override void Given()
        {
            var aggregate = new AccountAggregateRoot(AggregateId);
            aggregate.AddAmount(2.00M);
            Subject.Save(aggregate, aggregate.Version);

            ReloadedAccountAggregateRoot = Subject.GetById(AggregateId);
            ReloadedAccountAggregateRoot.AddAmount(5.00M);
            Subject.Save(ReloadedAccountAggregateRoot, ReloadedAccountAggregateRoot.Version);
        }

        protected override void When() => ReloadedAccountAggregateRoot = Subject.GetById(AggregateId);

        [Then]
        public void then_when_reloaded_the_account_contains_seven_pounds() => ReloadedAccountAggregateRoot.Balance.ShouldBe(7.00M);
    }
}