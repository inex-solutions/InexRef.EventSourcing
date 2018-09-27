using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Messages
{
    public interface IEvent : IMessage
    {
        int Version { get; }
    }
}