using System;

namespace Rob.EventSourcing.NaturalKey
{
    public class SqlServerNaturalKeyToAggregateIdMap<TNaturalKey, TInternalId> : INaturalKeyToAggregateIdMap<TNaturalKey, TInternalId>
    {
        public TInternalId this[TNaturalKey naturalKey]
        {
            get { throw new NotImplementedException(); }
        }

        public TInternalId GetOrCreateNew(TNaturalKey naturalKey)
        {
            throw new NotImplementedException();
        }
    }
}