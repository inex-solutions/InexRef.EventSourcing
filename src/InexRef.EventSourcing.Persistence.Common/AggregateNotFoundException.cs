using System;

namespace InexRef.EventSourcing.Persistence.Common
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