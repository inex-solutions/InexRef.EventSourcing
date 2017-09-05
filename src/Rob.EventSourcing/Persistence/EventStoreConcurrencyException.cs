using System;

namespace Rob.EventSourcing.Persistence
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