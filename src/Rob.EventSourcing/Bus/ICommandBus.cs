using System;
using Rob.ValuationMonitoring.EventSourcing.Messages;

namespace Rob.ValuationMonitoring.EventSourcing.Bus
{
    public interface ICommandBus
    {
        void RegisterHandler<T>(Action<T> handler) where T : Command;

        void Send<T>(T command) where T : Command;
    }
}