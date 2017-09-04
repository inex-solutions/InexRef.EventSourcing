using System;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Bus
{
    public class HandlerRegistrationException : Exception
    {
        public HandlerRegistrationException(string message) : base(message)
        {
        }
    }
}