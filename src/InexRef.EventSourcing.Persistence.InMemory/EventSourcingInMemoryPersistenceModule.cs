using InexRef.EventSourcing.Common.Container;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Persistence.InMemory
{
    public class EventSourcingInMemoryPersistenceModule : ContainerConfigurationModule
    {
        protected override void Load(IServiceCollection builder)
        {
            builder.RegisterModule<InMemoryPersistenceModule>();
        }
    }
}