using System;

namespace InexRef.EventSourcing.Contracts
{
    public interface IIdentifier<T> : IEquatable<T>, IComparable<T>
    {
    }
}