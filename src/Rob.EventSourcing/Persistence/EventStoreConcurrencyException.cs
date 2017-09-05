using System;

namespace Rob.ValuationMonitoring.EventSourcing.Persistence
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