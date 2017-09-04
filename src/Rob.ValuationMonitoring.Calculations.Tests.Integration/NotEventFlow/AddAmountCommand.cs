using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow
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