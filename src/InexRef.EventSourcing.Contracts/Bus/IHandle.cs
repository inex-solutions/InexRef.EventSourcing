using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Contracts.Bus
{
    public interface IHandle<in TMessage> where TMessage : IMessage
    {
        Task Handle(TMessage message);
    }
}
