namespace Rob.ValuationMonitoring.Calculation.NotEventFlow.Messages
{
    public abstract class Event : IMessage
    {
        public int Version { get; set; }
    }
}
