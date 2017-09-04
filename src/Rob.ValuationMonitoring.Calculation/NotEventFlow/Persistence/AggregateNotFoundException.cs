using System;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException()
        {

        }

        public AggregateNotFoundException(string message) : base(message)
        {

        }
    }
}