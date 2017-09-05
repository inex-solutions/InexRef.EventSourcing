namespace Rob.ValuationMonitoring.EventSourcing.Messages
{
    public abstract class Event : IMessage
    {
        public int Version { get; set; }
    }
}
