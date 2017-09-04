using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using Rob.ValuationMonitoring.Calculation.NotEventFlow;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow
{
    public abstract class AggregateRootTestBase<TAggregateRoot> : SpecificationBase where TAggregateRoot : new() 
    {
        protected TAggregateRoot Subject { get; set; }

        protected override void SetUp()
        {
            Subject = new TAggregateRoot();

        }
    }

    public class when_I_add_an_one_pound_to_an_empty_account : AggregateRootTestBase<AccountAggregateRoot>
    {
        protected override void Given() { }

        protected override void When() => Subject.AddAmount(1.00M);

        [Then]
        public void the_balance_is_one_pound() => Subject.Balance.ShouldBe(1.00M);
    }

    public class when_I_add_one_pound_to_an_account_containing_one_pound : AggregateRootTestBase<AccountAggregateRoot>
    {
        protected override void Given() => Subject.AddAmount(1.00M);

        protected override void When() => Subject.AddAmount(1.00M);

        [Then]
        public void the_balance_is_two_pounds() => Subject.Balance.ShouldBe(2.00M);
    }

    public class when_I_reset_an_account_containing_one_pound : AggregateRootTestBase<AccountAggregateRoot>
    {
        protected override void Given() => Subject.AddAmount(1.00M);

        protected override void When() => Subject.ResetBalance();

        [Then]
        public void the_balance_is_zero() => Subject.Balance.ShouldBe(0.00M);
    }

    public class AccountAggregateRoot : AggregateRoot
    {
        public override Guid Id { get; }

        public decimal Balance { get; set; }

        public void AddAmount(decimal amount)
        {
            Apply(new AddAmountEvent(amount));
        }

        public void ResetBalance()
        {
            Apply(new ResetBalanceEvent());
        }

        public void HandleEvent(AddAmountEvent @event, bool isNew)
        {
            Balance += @event.Amount;
        }

        public void HandleEvent(ResetBalanceEvent @event, bool isNew)
        {
            Balance = 0;
        }
    }

    public class AddAmountEvent : Event
    {
        public decimal Amount { get; }

        public AddAmountEvent(decimal amount)
        {
            Amount = amount;
        }
    }

    public class SubtractAmountEvent : Event
    {
        public decimal Amount { get; }

        public SubtractAmountEvent(decimal amount)
        {
            Amount = amount;
        }
    }

    public class ResetBalanceEvent : Event
    {
        
    }
}
