using Rob.ValuationMonitoring.Calculation.NotEventFlow;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow
{
    public class AddAmountEvent : Event
    {
        public decimal Amount { get; }

        public AddAmountEvent(decimal amount)
        {
            Amount = amount;
        }
    }
}