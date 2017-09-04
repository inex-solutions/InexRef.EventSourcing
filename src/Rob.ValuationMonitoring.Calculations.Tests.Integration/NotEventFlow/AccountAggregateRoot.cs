using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow
{
    public class AccountAggregateRoot : AggregateRoot
    {
        public AccountAggregateRoot()
        {
            
        }

        public AccountAggregateRoot(Guid id)
        {
            Id = id;
        }

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
}