using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Persistence.InMemory
{
    public class InMemoryPersistenceModule : ContainerConfigurationModule
    {
        protected override void Load(IServiceCollection builder)
        {
            builder.AddSingleton(typeof(IEventStore<>), typeof(InMemoryEventStore<>));

            builder.AddSingleton(typeof(INaturalKeyToAggregateIdMap<,,>), typeof(InMemoryNaturalKeyToAggregateIdMap<,,>));
        }
    }
}