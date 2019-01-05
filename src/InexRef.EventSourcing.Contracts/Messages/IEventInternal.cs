namespace InexRef.EventSourcing.Contracts.Messages
{
    public interface IEventInternal
    {
        void SetVersion(int version);
    }
}