using System;
using System.Linq;
using System.Threading;
using Autofac;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.EventStores;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;
using EventFlow.Queries;
using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ReadModels;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.WindowsHost
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UNHANDLED EXCEPTION {ex}");
            }
            finally
            {
                if (Environment.UserInteractive)
                {
                    Console.WriteLine("Press enter to exit");
                    Console.ReadLine();
                }
            }
        }

        private static async void Run(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            using (var resolver = EventFlowOptions.New
                .UseAutofacContainerBuilder(containerBuilder)
                .RegisterServices(sr => sr.RegisterType(typeof(LatestUnauditedPriceReadModelLocator)))
                .AddEvents(typeof(ValuationLineAggregate).Assembly)
                .AddCommandHandlers(typeof(ValuationLineAggregate).Assembly)
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(@"Server=localhost;Database=Rob.ValuationMonitoring;Trusted_Connection=True"))
                .UseEventStore<MsSqlEventPersistence>()
                .UseMssqlReadModel<LatestUnauditedPriceReadModel, LatestUnauditedPriceReadModelLocator>()
                .CreateResolver())
            {
                // uncomment the following to create the event flow schema
                //var msSqlDatabaseMigrator = resolver.Resolve<IMsSqlDatabaseMigrator>();
                //EventFlowEventStoresMsSql.MigrateDatabase(msSqlDatabaseMigrator);

                var commandBus = resolver.Resolve<ICommandBus>();
                var eventStore = resolver.Resolve<IEventStore>();

              //  var id = ValuationLineId.New;
                var id = new ValuationLineId("valuationline-64a102cb-0740-4f1a-a9ad-a4e92cad4ffb");

                // Publish a command
                UnauditedPrice price = new UnauditedPrice("PORG1", DateTime.Parse("11-Jan-2017"), "GBP", 5M);
                await commandBus.PublishAsync(new UpdateUnauditedPriceCommand(id, price), CancellationToken.None);

                // Resolve the query handler and use the built-in query for fetching
                // read models by identity to get our read model representing the
                // state of our aggregate root
                var queryProcessor = resolver.Resolve<IQueryProcessor>();
                var valuationLineReadModel = await queryProcessor.ProcessAsync(
                        new ReadModelByIdQuery<LatestUnauditedPriceReadModel>("PORG1"),
                        CancellationToken.None)
                    .ConfigureAwait(false);

                Console.WriteLine($"Unaudited Price from Read Model: {valuationLineReadModel.UnauditedPrice}");
                Console.WriteLine($"Events in Event Store: {eventStore.LoadEvents<ValuationLineAggregate, ValuationLineId>(id).Count}");
                Console.WriteLine($"First Event: {eventStore.LoadEvents<ValuationLineAggregate, ValuationLineId>(id).First()}");
            }
        }
    }
}
