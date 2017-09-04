using System;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages
{
    public abstract class Command : IMessage
    {
        protected Command(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}