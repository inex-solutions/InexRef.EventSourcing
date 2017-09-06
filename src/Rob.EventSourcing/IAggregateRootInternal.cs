using System;
using System.Collections.Generic;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing
{
    public interface IAggregateRootInternal
    {
        void Load(Guid id, IEnumerable<Event> history);
    }
}