using System;
using Rob.EventSourcing.Persistence;
using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.PersistenceTests
{
    public class when_a_saved_account_is_resaved_but_with_the_wrong_version_number : AggregateRepositoryTestBase
    {
        private Exception CaughtException;

        public when_a_saved_account_is_resaved_but_with_the_wrong_version_number(string persistenceProvider) : base(persistenceProvider) { }

        protected override void Given()
        {
            ReloadedAccountAggregateRoot = new NonDisposingAccountAggregateRoot(AggregateId);
            ReloadedAccountAggregateRoot.AddAmount(2.00M);

            // intermediate save should cause a concurrency error when we save below
            Subject.Save(ReloadedAccountAggregateRoot);

            ReloadedAccountAggregateRoot.AddAmount(5.00M);
        }

        protected override void When() => CaughtException = Catch.Exception (() => Subject.Save(ReloadedAccountAggregateRoot));

        [Then]
        public void a_EventStoreConcurrencyException_should_be_thrown() => CaughtException.ShouldBeOfType<EventStoreConcurrencyException>();
    }
}