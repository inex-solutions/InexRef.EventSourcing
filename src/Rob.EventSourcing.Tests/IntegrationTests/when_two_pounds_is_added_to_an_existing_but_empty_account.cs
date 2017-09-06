﻿using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class when_two_pounds_is_added_to_an_existing_but_empty_account : IntegrationTestBase
    {
        public when_two_pounds_is_added_to_an_existing_but_empty_account(string persistenceProvider) : base(persistenceProvider) { }

        protected override void Given()
        {
            var account = new AccountAggregateRoot(AggregateId);
            Repository.Save(account);
        }

        protected override void When() => Subject.Send(new AddAmountCommand(AggregateId, 2.00M));

        [Then]
        public void the_account_balance_is_two_pounds() => Repository.Get(AggregateId).Balance.ShouldBe(2.00M);

        [Then]
        public void the_account_balance_on_the_read_model_is_two_pounds() => BalanceReadModel[AggregateId].ShouldBe(2.00M);
    }
}