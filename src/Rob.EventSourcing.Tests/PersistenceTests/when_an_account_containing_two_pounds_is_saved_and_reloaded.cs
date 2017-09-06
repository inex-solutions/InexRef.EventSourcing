using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.PersistenceTests
{
    public class when_an_account_containing_two_pounds_is_saved_and_reloaded : AggregateRepositoryTestBase
    {
        protected override void Given()
        {
            var aggregate = new AccountAggregateRoot(AggregateId);
            aggregate.AddAmount(2.00M);
            Subject.Save(aggregate);
        }

        protected override void When() => ReloadedAccountAggregateRoot = Subject.Get(AggregateId);

        [Then]
        public void the_reloaded_account_contains_two_pounds() => ReloadedAccountAggregateRoot.Balance.ShouldBe(2.00M);
    }
}