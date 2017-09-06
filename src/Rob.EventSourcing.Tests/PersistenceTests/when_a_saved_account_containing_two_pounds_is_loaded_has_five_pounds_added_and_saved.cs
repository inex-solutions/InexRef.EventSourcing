using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.PersistenceTests
{
    public class when_a_saved_account_containing_two_pounds_is_loaded_has_five_pounds_added_and_saved : AggregateRepositoryTestBase
    {
        public when_a_saved_account_containing_two_pounds_is_loaded_has_five_pounds_added_and_saved(string persistenceProvider) : base(persistenceProvider) { }

        protected override void Given()
        {
            var aggregate = new AccountAggregateRoot(AggregateId);
            aggregate.AddAmount(2.00M);
            Subject.Save(aggregate);

            ReloadedAccountAggregateRoot = Subject.Get(AggregateId);
            ReloadedAccountAggregateRoot.AddAmount(5.00M);
            Subject.Save(ReloadedAccountAggregateRoot);
        }

        protected override void When() => ReloadedAccountAggregateRoot = Subject.Get(AggregateId);

        [Then]
        public void then_when_reloaded_the_account_contains_seven_pounds() => ReloadedAccountAggregateRoot.Balance.ShouldBe(7.00M);
    }
}