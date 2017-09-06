using Autofac;
using Rob.ValuationMonitoring.EventSourcing.Bus;
using Rob.ValuationMonitoring.EventSourcing.Persistence;

namespace Rob.ValuationMonitoring.EventSourcing
{
    public class EventSourcingInMemoryInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryBus>().As<IBus>().As<IEventBus>().As<ICommandBus>().SingleInstance();
            builder.RegisterGeneric(typeof(InMemoryEventStore<>)).As(typeof(IEventStore<>)).SingleInstance();
        }
    }
}