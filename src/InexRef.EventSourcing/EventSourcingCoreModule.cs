using System;
using InexRef.EventSourcing.Bus;
using InexRef.EventSourcing.Common;
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.OperationContext;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Domain;
using InexRef.EventSourcing.NaturalKey;
using InexRef.EventSourcing.Persistence.Common;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing
{
    public class EventSourcingCoreModule : ContainerConfigurationModule
    {
        protected override void Load(IServiceCollection builder)
        {
            builder.AddTransient<IBus, InMemoryBus>();
            builder.AddTransient<IAggregateRootFactory, AggregateRootFactory>();
            builder.AddTransient<IAggregateIdCreator<Guid>, GuidAggregateIdCreator>();
            builder.AddTransient(typeof(IAggregateRepository<,>), typeof(AggregateRepository<,>));
            builder.AddTransient(typeof(INaturalKeyDrivenAggregateRepository<,,>), typeof(NaturalKeyDrivenAggregateRepository<,,>));
            
            builder.AddScoped<OperationContext>()
                .AddScoped<IOperationContext>(provider => provider.GetRequiredService<OperationContext>())
                .AddScoped<IWriteableOperationContext>(provider => provider.GetRequiredService<OperationContext>());

            builder.AddTransient<IUpdateOperationContext, OperationContextUpdater>();

            builder.AddTransient<IDateTimeProvider, DateTimeProvider>();
        }
    }
}