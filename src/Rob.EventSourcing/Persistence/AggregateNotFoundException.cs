using System;

namespace Rob.ValuationMonitoring.EventSourcing.Persistence
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