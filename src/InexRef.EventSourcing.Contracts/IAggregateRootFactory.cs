namespace InexRef.EventSourcing.Contracts
{
    public interface IAggregateRootFactory
    {
        TAggregateRoot Create<TAggregateRoot>();
    }
}