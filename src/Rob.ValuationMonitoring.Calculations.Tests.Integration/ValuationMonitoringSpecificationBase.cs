using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Autofac.Extensions;
using EventFlow.Commands;
using EventFlow.Configuration;
using EventFlow.Core;
using EventFlow.EventStores;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;
using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.ReadModels;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration
{
    public abstract class ValuationMonitoringSpecificationBase : SpecificationBaseAsync
    {
        private static int _count;

        protected ValuationLineId ValuationLineId { get; private set; }

        protected static IRootResolver Resolver { get; private set; }

        protected static ICommandBus CommandBus { get; private set; }

        protected static IEventStore EventStore { get; private set; }

        protected static IAggregateStore AggregateStore { get; private set; }

        protected AsOfQueries AsOfQueries { get; private set; }

        static ValuationMonitoringSpecificationBase()
        {
            var containerBuilder = new ContainerBuilder();

            Resolver = EventFlowOptions.New
                .UseAutofacContainerBuilder(containerBuilder)
                .AddEvents(typeof(ValuationLineAggregate).Assembly)
                .AddCommandHandlers(typeof(ValuationLineAggregate).Assembly)
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(@"Server=localhost;Database=Rob.ValuationMonitoring;Trusted_Connection=True"))
                .UseEventStore<MsSqlEventPersistence>()
                .UseMssqlReadModel<LatestUnauditedPriceReadModel>()
                .UseInMemoryReadStoreFor<RawEventReadModel>()
                .AddSnapshots(typeof(ValuationLineSnapshot))
                .UseMsSqlSnapshotStore()
                .CreateResolver();

            CommandBus = Resolver.Resolve<ICommandBus>();
            EventStore = Resolver.Resolve<IEventStore>();
            AggregateStore = Resolver.Resolve<IAggregateStore>();
        }

        protected override void SetUp()
        {
            ValuationLineId = CreateValuationLineId();
            AsOfQueries = new AsOfQueries(Resolver);
        }

        protected IReadOnlyCollection<IDomainEvent<ValuationLineAggregate, ValuationLineId>> GetEventsFromStore(ValuationLineId valuationLineId)
        {
            return EventStore.LoadEventsAsync<ValuationLineAggregate, ValuationLineId>(valuationLineId, CancellationToken.None).GetAwaiter().GetResult();
        }

        protected ValuationLineAggregate GetAggregate(ValuationLineId valuationLineId)
        {
            return AggregateStore.LoadAsync<ValuationLineAggregate, ValuationLineId>(valuationLineId, CancellationToken.None).GetAwaiter().GetResult();
        }

        protected virtual async Task Publish(ICommand<ValuationLineAggregate, ValuationLineId, ISourceId> command)
        {
            await CommandBus.PublishAsync(command, CancellationToken.None) .ConfigureAwait(false);
        }

        protected ValuationLineId CreateValuationLineId()
        {
            var count = Interlocked.Increment(ref _count);
            return new ValuationLineId($"PORG-{DateTime.Now:yyyyMMddHHmmssfff}-{count}");
        }
    }
}