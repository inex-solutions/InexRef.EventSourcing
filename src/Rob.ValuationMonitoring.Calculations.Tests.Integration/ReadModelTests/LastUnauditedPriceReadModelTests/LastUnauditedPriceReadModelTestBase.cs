using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using EventFlow.Core;
using EventFlow.Queries;
using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.ReadModels;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.ReadModelTests.LastUnauditedPriceReadModelTests
{
    public abstract class LastUnauditedPriceReadModelTestBase : ValuationMonitoringSpecificationBase
    {
        private LatestUnauditedPriceReadModel latestUnauditedPriceReadModel;

        protected UnauditedPrice Price { get; set; }

        protected LatestUnauditedPriceReadModel LatestUnauditedPriceReadModel
        {
            get
            {
                if (latestUnauditedPriceReadModel == null)
                {
                    var queryProcessor = Resolver.Resolve<IQueryProcessor>();
                    latestUnauditedPriceReadModel = queryProcessor.ProcessAsync(
                            new ReadModelByIdQuery<LatestUnauditedPriceReadModel>(ValuationLineId),
                            CancellationToken.None)
                        .GetAwaiter().GetResult();
                }

                return latestUnauditedPriceReadModel;
            }
        }

        protected override async Task Publish(ICommand<ValuationLineAggregate, ValuationLineId, ISourceId> command)
        {
            ResetLatestUnauditedPriceReadModel();
            await base.Publish(command);
        }

        protected void ResetLatestUnauditedPriceReadModel()
        {
            latestUnauditedPriceReadModel = null;
        }
    }
}