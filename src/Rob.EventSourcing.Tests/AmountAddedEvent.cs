using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class AmountAddedEvent : Event<Guid>
    {
        public decimal Amount { get; }

        public AmountAddedEvent(Guid id, decimal amount) : base(id)
        {
            Amount = amount;
        }
    }
}