using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class AddAmountCommand : Command
    {
        public decimal Amount { get; }

        public AddAmountCommand(Guid id, decimal amount) : base(id)
        {
            Amount = amount;
        }
    }
}