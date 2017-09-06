using System;
using System.Diagnostics;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    [DebuggerDisplay("BalanceUpdatedEvent: Id={Id}, Balance={Balance}, Version={Version}")]
    public class BalanceUpdatedEvent : Event
    {
        public decimal Balance { get; }

        public BalanceUpdatedEvent(Guid id, decimal balance) : base(id)
        {
            Balance = balance;
        }
    }
}