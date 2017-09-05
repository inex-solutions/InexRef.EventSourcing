using System;
using Rob.EventSourcing.Persistence;
using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.PersistenceTests
{
    public class when_a_saved_account_is_resaved_but_with_the_wrong_version_number : AggregateRepositoryTestBase
    {
        private Exception CaughtException;

        protected override void Given()
        {
            var aggregate = new AccountAggregateRoot(AggregateId);
            aggregate.AddAmount(2.00M);
            Subject.Save(aggregate, aggregate.Version);

            ReloadedAccountAggregateRoot = Subject.Get(AggregateId);
            ReloadedAccountAggregateRoot.AddAmount(5.00M);
        }

        protected override void When() => CaughtException = Catch.Exception (() => Subject.Save(ReloadedAccountAggregateRoot, ReloadedAccountAggregateRoot.Version - 1));

        [Then]
        public void a_EventStoreConcurrencyException_should_be_thrown() => CaughtException.ShouldBeOfType<EventStoreConcurrencyException>();
    }
}