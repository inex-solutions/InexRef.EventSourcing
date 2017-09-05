using System;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing
{
    public class NullBus : IBus
    {
        public void RegisterHandler<T>(Action<T> handler) where T : Command
        {
        }

        public void Send<T>(T command) where T : Command
        {
        }

        public void Subscribe<T>(Action<T> handler) where T : Event
        {
        }

        public void PublishEvent(Event @event)
        {
        }
    }
}