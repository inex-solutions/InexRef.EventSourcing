using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages;

namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Bus
{
    public interface ICommandBus
    {
        void RegisterHandler<T>(Action<T> handler) where T : Command;

        void Send<T>(T command) where T : Command;
    }
}