using System;
using System.Linq;
using System.Threading;
using Autofac;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.EventStores;
using EventFlow.Extensions;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;
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
            containerBuilder.RegisterType<Test>().As<ITest>();

            using (var resolver = EventFlowOptions.New
                .UseAutofacContainerBuilder(containerBuilder)
                .AddEvents(typeof(ValuationLineAggregate).Assembly)
                .AddCommandHandlers(typeof(ValuationLineAggregate).Assembly)
                .UseInMemoryReadStoreFor<ValuationLineReadModel>()
                .CreateResolver())
            {
                var commandBus = resolver.Resolve<ICommandBus>();
                var eventStore = resolver.Resolve<IEventStore>();
                var readModelStore = resolver.Resolve<IInMemoryReadStore<ValuationLineReadModel>>();
                
                var id = new ValuationLineId("PORG1");

                // Publish a command
                UnauditedPrice price = new UnauditedPrice(DateTime.Now, "GBP", 12.34M);
                await commandBus.PublishAsync(new UpdateUnauditedPriceCommand(id, price), CancellationToken.None);

                // Resolve the query handler and use the built-in query for fetching
                // read models by identity to get our read model representing the
                // state of our aggregate root
                var queryProcessor = resolver.Resolve<IQueryProcessor>();
                var valuationLineReadModel = await queryProcessor.ProcessAsync(
                        new ReadModelByIdQuery<ValuationLineReadModel>(id),
                        CancellationToken.None)
                    .ConfigureAwait(false);

                Console.WriteLine($"Unaudited Price from Read Model: {valuationLineReadModel.UnauditedPrice}");
                Console.WriteLine($"Events in Event Store: {eventStore.LoadEvents<ValuationLineAggregate, ValuationLineId>(id).Count}");
                Console.WriteLine($"First Event: {eventStore.LoadEvents<ValuationLineAggregate, ValuationLineId>(id).First()}");
                var test = resolver.Resolve<ITest>();
                Console.WriteLine($"Test class resolution resolved: {test}");
            }
        }
    }

    public interface ITest
    {
        
    }

    public class Test : ITest
    {
        
    }
}
