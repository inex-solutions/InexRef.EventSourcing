using System.Collections.Generic;
using System.Threading;
using Autofac;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Autofac.Extensions;
using EventFlow.Configuration;
using EventFlow.EventStores;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;
using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.ReadModels;
using Rob.ValuationMonitoring.Calculations.Tests.Unit.SpecificationTests;

namespace Rob.ValuationMonitoring.Calculations.Tests.Unit
{
    public abstract class ValuationMonitoringSpecificationBase : SpecificationBaseAsync
    {
        protected Calculation.ValuationLineId Id { get; set; }

        protected IRootResolver Resolver { get; private set; }

        protected ICommandBus CommandBus { get; private set; }

        protected IEventStore EventStore { get; private set; }

        protected IAggregateStore AggregateStore { get; private set; }

        protected override void SetUp()
        {
            var containerBuilder = new ContainerBuilder();

            Resolver = EventFlowOptions.New
                .UseAutofacContainerBuilder(containerBuilder)
                .RegisterServices(sr => sr.RegisterType(typeof(ValuationLineLocator)))
                .AddEvents(typeof(ValuationLineAggregate).Assembly)
                .AddCommandHandlers(typeof(ValuationLineAggregate).Assembly)
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(@"Server=localhost;Database=Rob.ValuationMonitoring;Trusted_Connection=True"))
                .UseEventStore<MsSqlEventPersistence>()
             //   .UseMssqlReadModel<ValuationLineReadModel, ValuationLineLocator>()
                .CreateResolver();

            CommandBus = Resolver.Resolve<ICommandBus>();
            EventStore = Resolver.Resolve<IEventStore>();
            AggregateStore = Resolver.Resolve<IAggregateStore>();
        }

        protected override void Cleanup()
        {
            Resolver.Dispose();
        }

        protected IReadOnlyCollection<IDomainEvent<ValuationLineAggregate, ValuationLineId>> GetEventsFromStore(ValuationLineId valuationLineId)
        {
            return EventStore.LoadEventsAsync<ValuationLineAggregate, ValuationLineId>(valuationLineId, CancellationToken.None).GetAwaiter().GetResult();
        }

        protected ValuationLineAggregate GetAggregate(ValuationLineId valuationLineId)
        {
            return AggregateStore.LoadAsync<ValuationLineAggregate, ValuationLineId>(valuationLineId, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}