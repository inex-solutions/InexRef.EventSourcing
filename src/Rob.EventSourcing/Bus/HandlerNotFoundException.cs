using System;

namespace Rob.EventSourcing.Bus
{
    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException(string message) : base (message)
        {
        }
    }
}