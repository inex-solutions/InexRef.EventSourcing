using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Common.Container
{
    public abstract class ContainerConfigurationModule
    {
        //RJL - put check to ensure never loaded twice
        public void ConfigureContainer(IServiceCollection serviceCollection)
        {
            Load(serviceCollection);
        }

        protected abstract void Load(IServiceCollection containerBuilder);
    }
}