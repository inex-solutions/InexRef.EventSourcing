using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Contracts.Bus
{
    public interface ICommandBus
    {
        Task Send(ICommand command);
    }
}