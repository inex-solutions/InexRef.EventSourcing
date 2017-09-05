using System;

namespace Rob.ValuationMonitoring.EventSourcing.Bus
{
    public class HandlerRegistrationException : Exception
    {
        public HandlerRegistrationException(string message) : base(message)
        {
        }
    }
}