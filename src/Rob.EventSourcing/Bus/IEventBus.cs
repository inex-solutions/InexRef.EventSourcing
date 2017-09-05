using System;
using Rob.ValuationMonitoring.EventSourcing.Messages;

namespace Rob.ValuationMonitoring.EventSourcing.Bus
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler) where T : Event;

        void PublishEvent<T>(T @event) where T : Event;
    }
}