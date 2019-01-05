using System;

namespace InexRef.EventSourcing.Contracts.Bus
{
    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException(string message) : base (message)
        {
        }
    }
}