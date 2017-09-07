namespace Rob.EventSourcing.Messages
{
    public interface IEventInternal
    {
        void SetVersion(int version);
    }
}