using Autofac;
using Rob.EventSourcing.Persistence;

namespace Rob.EventSourcing
{
    public class EventSourcingCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(AggregateRepository<>)).As(typeof(IAggregateRepository<>));
        }
    }
}