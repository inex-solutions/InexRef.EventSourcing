using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class ResetBalanceCommand : Command
    {
        public ResetBalanceCommand(Guid id) : base(id)
        {
        }
    }
}