using System;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Bus
{
    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException(string message) : base (message)
        {
        }
    }
}