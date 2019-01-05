using InexRef.EventSourcing.Common;
using InexRef.EventSourcing.Common.Hosting;
using InexRef.EventSourcing.Persistence.InMemory;
using InexRef.EventSourcing.Persistence.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Tests.Common
{
    public static class TestEnvironmentSetup
    {
        public static void ConfigureContainerForHostEnvironmentFlavour(ServiceCollection containerBuilder, string flavour)
        {
            ImportAssemblyContaining<InMemoryPersistenceModule>();
            ImportAssemblyContaining<EventSourcingSqlServerPersistenceModule>();

            HostedEnvironmentFlavour.ConfigureContainerForHostEnvironmentFlavour(containerBuilder, flavour);

            containerBuilder
                .AddSingleton<IDateTimeProvider, DeterministicallyIncreasingDateTimeProvider>();
        }

        private static void ImportAssemblyContaining<T>()
        {
            // workaround - does nothing, but the explicit reference to type T ensures the assembly is imported. The 
            // alternative is to dynamically load the assembly from, or copy it into the current directory. 
        }
    }
}