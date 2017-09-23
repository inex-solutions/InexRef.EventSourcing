using System;

namespace Rob.EventSourcing.NaturalKey
{
    public class GuidAggregateIdCreator : IAggregateIdCreator<Guid>
    {
        public Guid Create() => Guid.NewGuid();
    }
}