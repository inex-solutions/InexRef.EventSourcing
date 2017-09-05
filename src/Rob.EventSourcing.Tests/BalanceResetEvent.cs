using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class BalanceResetEvent : Event
    {
        public BalanceResetEvent(Guid id) : base(id)
        {
        }
    }
}