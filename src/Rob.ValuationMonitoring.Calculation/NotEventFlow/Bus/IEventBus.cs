using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Bus
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler) where T : Event;

        void PublishEvent<T>(T @event) where T : Event;
    }
}