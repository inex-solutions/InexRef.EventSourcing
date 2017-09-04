using System;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence
{
    public class EventStoreConcurrencyException : Exception
    {
        public EventStoreConcurrencyException()
        {
            
        }

        public EventStoreConcurrencyException(string message) : base(message)
        {
            
        }
    }
}