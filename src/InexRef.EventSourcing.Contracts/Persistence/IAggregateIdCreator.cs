namespace InexRef.EventSourcing.Contracts.Persistence
{
    public interface IAggregateIdCreator<out T>
    {
        T Create();
    }
}