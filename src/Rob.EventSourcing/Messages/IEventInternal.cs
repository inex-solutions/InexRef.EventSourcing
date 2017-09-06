namespace Rob.ValuationMonitoring.EventSourcing.Messages
{
    public interface IEventInternal
    {
        void SetVersion(int version);
    }
}