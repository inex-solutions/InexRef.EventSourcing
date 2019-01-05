using System;

namespace InexRef.EventSourcing.Common
{
    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();
    }
}