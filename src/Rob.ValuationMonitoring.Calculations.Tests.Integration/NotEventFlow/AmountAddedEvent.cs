using Rob.ValuationMonitoring.Calculation.NotEventFlow;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow
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