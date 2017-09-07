using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class BalanceResetEvent : Event<string>
    {
        public BalanceResetEvent(string id) : base(id)
        {
        }
    }
}