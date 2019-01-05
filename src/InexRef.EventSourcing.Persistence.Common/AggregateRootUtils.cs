using System;

namespace InexRef.EventSourcing.Persistence.Common
{
    public static class AggregateRootUtils
    {
        public static string GetAggregateRootName<TAggregateRoot>()
        {
            return GetAggregateRootName(typeof(TAggregateRoot));
        }

        public static string GetAggregateRootName(Type aggregateRootType)
        {
            return aggregateRootType.Name.Replace("AggregateRoot", "");
        }
    }
}
