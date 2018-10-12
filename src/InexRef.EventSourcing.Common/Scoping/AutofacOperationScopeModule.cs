using Autofac;

namespace InexRef.EventSourcing.Common.Scoping
{

    public class AutofacOperationScopeModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<AutofacOperationScopeManager>()
                .As<IOperationScopeManager>();

            containerBuilder
                .RegisterType<AutofacOperationScope>()
                .As<IOperationScope>()
                .As<IOperationScopeInternal>()
                .InstancePerLifetimeScope();
        }
    }
}
