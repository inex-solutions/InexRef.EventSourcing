using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Bus
{
    public interface ICommandBus
    {
        void RegisterHandler<T>(Action<T> handler) where T : Command;

        void Send<T>(T command) where T : Command;
    }
}