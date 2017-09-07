using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class AddAmountCommand : Command<string>
    {
        public decimal Amount { get; }

        public AddAmountCommand(string id, decimal amount) : base(id)
        {
            Amount = amount;
        }
    }
}