using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class AmountAddedEvent : Event<string>
    {
        public decimal Amount { get; }

        public AmountAddedEvent(string id, decimal amount) : base(id)
        {
            Amount = amount;
        }
    }
}