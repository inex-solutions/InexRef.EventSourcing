using Autofac;
using Rob.EventSourcing.Bus;
using Rob.EventSourcing.Persistence;

namespace Rob.EventSourcing
{
    public class EventSourcingInMemoryInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryBus>().As<IBus>().As<IEventBus>().As<ICommandBus>().SingleInstance();
            builder.RegisterType(typeof(InMemoryEventStore)).As(typeof(IEventStore)).SingleInstance();
        }
    }
}