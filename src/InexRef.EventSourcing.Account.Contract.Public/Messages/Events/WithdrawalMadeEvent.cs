using System;
using InexRef.EventSourcing.Account.Contract.Public.Types;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Account.Contract.Public.Messages.Events
{
    public class WithdrawalMadeEvent : Event<Guid>
    {
        public MonetaryAmount Amount { get; }

        public WithdrawalMadeEvent(MessageMetadata messageMetadata, Guid id, MonetaryAmount amount) : base(messageMetadata, id)
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: MessageMetadata={MessageMetadata}, Id={Id}, Amount={Amount}, Version={Version}";
        }
    }
}