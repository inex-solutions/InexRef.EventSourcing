using System;

namespace InexRef.EventSourcing.Persistence.EventStore
{
    public class EventStoreDeletionFailedException : Exception
    {
        public EventStoreDeletionFailedException(string message) : base(message)
        {
            
        }
    }
}