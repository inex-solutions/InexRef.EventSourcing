using System;
using Rob.EventSourcing.Tests.IntegrationTests;

namespace Rob.EventSourcing.Tests
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
            Apply(new AmountAddedEvent(Id, amount));
        }

        public void ResetBalance()
        {
            Apply(new BalanceResetEvent(Id));
        }

        public void HandleEvent(AmountAddedEvent @event, bool isNew)
        {
            Balance += @event.Amount;
            Bus.PublishEvent(new BalanceUpdatedEvent(Id, Balance));
        }

        public void HandleEvent(BalanceResetEvent @event, bool isNew)
        {
            Balance = 0;
            Bus.PublishEvent(new BalanceResetEvent(Id));
        }
    }
}