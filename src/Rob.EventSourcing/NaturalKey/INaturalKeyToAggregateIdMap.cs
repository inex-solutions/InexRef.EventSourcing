namespace Rob.EventSourcing.NaturalKey
{
    public interface INaturalKeyToAggregateIdMap<in TNaturalKey, out TInternalId, TAggregate>
    {
        TInternalId this[TNaturalKey naturalKey] { get; }

        TInternalId GetOrCreateNew(TNaturalKey naturalKey);

        void Delete(TNaturalKey naturalKey);
    }
}