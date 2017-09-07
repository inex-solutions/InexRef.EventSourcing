using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class BalanceResetEvent : Event<Guid>
    {
        public BalanceResetEvent(Guid id) : base(id)
        {
        }
    }
}