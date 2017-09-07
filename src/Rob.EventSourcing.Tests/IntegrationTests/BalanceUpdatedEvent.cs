using System;
using System.Diagnostics;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    [DebuggerDisplay("BalanceUpdatedEvent: Id={Id}, Balance={Balance}, Version={Version}")]
    public class BalanceUpdatedEvent : Event<string>
    {
        public decimal Balance { get; }

        public BalanceUpdatedEvent(string id, decimal balance) : base(id)
        {
            Balance = balance;
        }
    }
}