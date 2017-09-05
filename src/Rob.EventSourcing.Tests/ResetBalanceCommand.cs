using System;
using Rob.ValuationMonitoring.EventSourcing.Messages;

namespace Rob.ValuationMonitoring.EventSourcing.Tests
{
    public class ResetBalanceCommand : Command
    {
        public ResetBalanceCommand(Guid id) : base(id)
        {
        }
    }
}