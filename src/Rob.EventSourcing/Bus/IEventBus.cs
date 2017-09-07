using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Bus
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler) where T : IEvent;

        void PublishEvent(IEvent @event);
    }
}