using System;
using InexRef.EventSourcing.Contracts.Persistence;

namespace InexRef.EventSourcing.NaturalKey
{
    public class GuidAggregateIdCreator : IAggregateIdCreator<Guid>
    {
        public Guid Create() => Guid.NewGuid();
    }
}