namespace Rob.EventSourcing.NaturalKey
{
    public interface IAggregateIdCreator<out T>
    {
        T Create();
    }
}