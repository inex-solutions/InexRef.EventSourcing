using System;
using Rob.ValuationMonitoring.EventSourcing.Messages;

namespace Rob.ValuationMonitoring.EventSourcing.Tests
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