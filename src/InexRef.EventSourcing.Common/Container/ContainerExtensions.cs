using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Common.Container
{
    public static class ContainerExtensions
    {
        public static void RegisterModule<TModule>(this IServiceCollection serviceCollection)
            where TModule : ContainerConfigurationModule, new()
        {
            var module = new TModule();
            module.ConfigureContainer(serviceCollection);
        }
    }
}