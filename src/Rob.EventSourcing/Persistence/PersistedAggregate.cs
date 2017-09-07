using System;
using System.Collections.Generic;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Persistence
{
    public class PersistedAggregate<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        public Metadata<TId> Metadata { get; set; }

        public IEnumerable<IEvent<TId>> Events { get; set; }
    }
}