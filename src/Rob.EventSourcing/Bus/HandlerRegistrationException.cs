using System;

namespace Rob.EventSourcing.Bus
{
    public class HandlerRegistrationException : Exception
    {
        public HandlerRegistrationException(string message) : base(message)
        {
        }
    }
}