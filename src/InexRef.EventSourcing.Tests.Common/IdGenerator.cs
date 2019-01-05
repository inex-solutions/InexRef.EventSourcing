using System;
using System.Diagnostics;
using System.Threading;

namespace InexRef.EventSourcing.Tests.Common
{
    public class IdGenerator
    {
        private static int _count;
        private readonly string _prefix;

        public IdGenerator(string prefix)
        {
            _prefix = prefix;
        }

        public string CreateAggregateId()
        {
            var count = Interlocked.Increment(ref _count);
            var id = $"{_prefix}-{DateTime.UtcNow:yyyyMMddHHmmss}-{count:D2}-{Process.GetCurrentProcess().Id}-{Thread.CurrentThread.ManagedThreadId}";
            return id;
        }
    }
}