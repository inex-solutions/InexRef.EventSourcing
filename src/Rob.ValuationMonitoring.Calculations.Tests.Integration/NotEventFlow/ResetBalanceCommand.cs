using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow
{
    public class ResetBalanceCommand : Command
    {
        public ResetBalanceCommand(Guid id) : base(id)
        {
        }
    }
}