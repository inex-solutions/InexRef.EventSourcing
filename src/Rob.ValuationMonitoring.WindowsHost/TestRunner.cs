using System;
using System.Threading.Tasks;
using Autofac;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.Configuration;
using EventFlow.Extensions;
using EventFlow.Logs;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;
using EventFlow.MsSql.SnapshotStores;
using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.ReadModels;

namespace Rob.ValuationMonitoring.WindowsHost
{
    public class TestRunner : IDisposable
    {

        private IRootResolver _resolver;

        public void SetupEnvironment()
        {
            var containerBuilder = new ContainerBuilder();

            _resolver = EventFlowOptions.New
                .UseAutofacContainerBuilder(containerBuilder)
                .AddEvents(typeof(ValuationLineAggregate).Assembly)
                .AddCommandHandlers(typeof(ValuationLineAggregate).Assembly)
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(@"Server=localhost;Database=Rob.ValuationMonitoring;Trusted_Connection=True"))
                .UseEventStore<MsSqlEventPersistence>()
                .UseMssqlReadModel<LatestUnauditedPriceReadModel>()
                .UseInMemoryReadStoreFor<RawEventReadModel>()
                .RegisterServices(r => r.Register(typeof(ILog), typeof(InfoConsoleLog)))
                .AddSnapshots(typeof(ValuationLineSnapshot))
                .UseMsSqlSnapshotStore()
                .CreateResolver();

            // uncomment the following to create the event flow schema
            //var msSqlDatabaseMigrator = _resolver.Resolve<IMsSqlDatabaseMigrator>();
            //EventFlowEventStoresMsSql.MigrateDatabase(msSqlDatabaseMigrator);
            //EventFlowSnapshotStoresMsSql.MigrateDatabase(msSqlDatabaseMigrator);
        }

        public async Task ExecuteTest(ITest test)
        {
            test.Init(_resolver);
            await test.Execute();
        }

        public void Dispose()
        {
            _resolver?.Dispose();
        }
    }
}