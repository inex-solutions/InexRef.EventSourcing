using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow;
using Rob.ValuationMonitoring.EventSourcing;
using Rob.ValuationMonitoring.EventSourcing.Tests;

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
            Apply(new AmountAddedEvent(amount));
        }

        public void ResetBalance()
        {
            Apply(new BalanceResetEvent());
        }

        public void HandleEvent(AmountAddedEvent @event, bool isNew)
        {
            Balance += @event.Amount;
        }

        public void HandleEvent(BalanceResetEvent @event, bool isNew)
        {
            Balance = 0;
        }
    }
}