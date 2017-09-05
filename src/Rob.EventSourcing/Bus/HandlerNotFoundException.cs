using System;

namespace Rob.ValuationMonitoring.EventSourcing.Bus
{
    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException(string message) : base (message)
        {
        }
    }
}