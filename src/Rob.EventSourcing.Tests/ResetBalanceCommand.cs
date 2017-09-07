using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class ResetBalanceCommand : Command<string>
    {
        public ResetBalanceCommand(string id) : base(id)
        {
        }
    }
}