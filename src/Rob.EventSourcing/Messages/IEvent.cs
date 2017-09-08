namespace Rob.EventSourcing.Messages
{
    public interface IEvent : IMessage
    {
        int Version { get; }
    }
}