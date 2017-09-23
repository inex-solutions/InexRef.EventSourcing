using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Tests
{
    public class AccountIdSetEvent : Event<Guid>
    {
        public string AccountId { get; }

        public AccountIdSetEvent(Guid id, string accountId) : base(id)
        {
            AccountId = accountId;
        }

        public override string ToString()
        {
            return $"AccountIdSetEvent: Id={Id}, AccountId={AccountId}, Version={Version}";
        }
    }
}