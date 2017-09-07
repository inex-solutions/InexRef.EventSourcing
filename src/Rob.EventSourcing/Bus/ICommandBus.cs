using System;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Bus
{
    public interface ICommandBus
    {
        void RegisterHandler<T>(Action<T> handler) where T : ICommand;

        void Send(ICommand command);
    }
}