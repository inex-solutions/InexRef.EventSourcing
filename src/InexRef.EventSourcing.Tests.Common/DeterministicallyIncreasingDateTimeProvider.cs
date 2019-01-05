using System;
using System.Threading;
using InexRef.EventSourcing.Common;

namespace InexRef.EventSourcing.Tests.Common
{
    public class DeterministicallyIncreasingDateTimeProvider : IDateTimeProvider
    {
        private readonly Lazy<DateTime> _lazyDateTime = new Lazy<DateTime>(() => DateTime.UtcNow);
        private int _numCalls;

        public DateTime GetUtcNow()
        {
            var dt = _lazyDateTime.Value;
            var numCalls = Interlocked.Increment(ref _numCalls);
            return dt.AddSeconds(numCalls);
        }
    }
}