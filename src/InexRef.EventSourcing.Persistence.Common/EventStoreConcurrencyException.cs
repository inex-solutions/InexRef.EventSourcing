using System;

namespace InexRef.EventSourcing.Persistence.Common
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