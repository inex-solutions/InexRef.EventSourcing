namespace Rob.EventSourcing.NaturalKey
{
    public interface INaturalKeyToAggregateIdMap<in TNaturalKey, out TInternalId>
    {
        TInternalId this[TNaturalKey naturalKey] { get; }

        TInternalId GetOrCreateNew(TNaturalKey naturalKey);
    }
}