namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class BalanceEntry
    {
        public decimal Balance { get; set; }

        public int Version { get; set; }
    }
}