namespace InexRef.EventSourcing.Contracts.Messages
{
    public interface IMessage
    {
        MessageMetadata MessageMetadata { get; }
    }
}