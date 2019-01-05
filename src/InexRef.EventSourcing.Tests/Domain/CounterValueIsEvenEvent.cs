using System;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class CounterValueIsEvenEvent : Event<Guid>
    {
        public int CurrentValue { get; }

        public CounterValueIsEvenEvent(MessageMetadata metadata, Guid id, int currentValue) : base(metadata, id)
        {
            CurrentValue = currentValue;
        }
    }
}