using Rob.ValuationMonitoring.EventSourcing.Messages;

namespace Rob.ValuationMonitoring.EventSourcing.Tests
{
    public class AmountAddedEvent : Event
    {
        public decimal Amount { get; }

        public AmountAddedEvent(decimal amount)
        {
            Amount = amount;
        }
    }
}