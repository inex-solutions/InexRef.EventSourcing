using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Contracts.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class CounterDomainHostModule : ContainerConfigurationModule
    {
        protected override void Load(IServiceCollection containerBuilder)
        {
            containerBuilder.RegisterModule<EventSourcingCoreModule>();

            containerBuilder.AddTransient<CounterDomainTestHandlers>();
            containerBuilder
                .AddTransient<IHandle<InitialiseCounterCommand>>(container => container.GetRequiredService<CounterDomainTestHandlers>())
                .AddTransient<IHandle<IncrementCounterCommand>>(container => container.GetRequiredService<CounterDomainTestHandlers>());
        }
    }
}