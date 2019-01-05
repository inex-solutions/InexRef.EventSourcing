using System.Collections.Generic;
using System.Linq;
using InexRef.EventSourcing.Common.Hosting;

namespace InexRef.EventSourcing.Tests.Common
{
    public static class NUnitTestFixtureSource
    {
        public static IEnumerable<object[]> TestFlavours =>
            HostedEnvironmentFlavour.AvailableFlavours.Select(f => new object[] { f }).ToArray();
    }
}