using System;

namespace Rob.EventSourcing.Persistence
{
    public class Metadata<TId> where TId : IEquatable<TId>, IComparable<TId>
    {
        public TId Id { get; set; }

        public string AggregateType { get; set; }

        public int Version { get; set; }
    }
}