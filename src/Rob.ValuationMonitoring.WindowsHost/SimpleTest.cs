using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.Queries;
using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ReadModels;

namespace Rob.ValuationMonitoring.WindowsHost
{
    public class SimpleTest : TestBase
    {
        public override async Task Execute()
        {
            var id = new ValuationLineId("PORG1");

            // Publish a command
            var priceCommand = new UpdateUnauditedPriceCommand(id, $"InitialName-{id}", DateTime.Parse("14-Jan-2017"), "GBP", 7M, DateTime.Now);
            await CommandBus.PublishAsync(priceCommand, CancellationToken.None);

            // Resolve the query handler and use the built-in query for fetching
            // read models by identity to get our read model representing the
            // state of our aggregate root
            var queryProcessor = Resolver.Resolve<IQueryProcessor>();
            var valuationLineReadModel = await queryProcessor.ProcessAsync(
                    new ReadModelByIdQuery<LatestUnauditedPriceReadModel>(id.Value),
                    CancellationToken.None)
                .ConfigureAwait(false);

            Console.WriteLine($"Unaudited Price from Read Model: {valuationLineReadModel.UnauditedPrice}");
            Console.WriteLine($"Events in Event Store: {EventStore.LoadEvents<ValuationLineAggregate, ValuationLineId>(id).Count}");
            Console.WriteLine($"First Event: {EventStore.LoadEvents<ValuationLineAggregate, ValuationLineId>(id).First()}");
        }
    }
}