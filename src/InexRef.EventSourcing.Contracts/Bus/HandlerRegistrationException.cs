using System;

namespace InexRef.EventSourcing.Contracts.Bus
{
    public class HandlerRegistrationException : Exception
    {
        public HandlerRegistrationException(string message) : base(message)
        {
        }
    }
}