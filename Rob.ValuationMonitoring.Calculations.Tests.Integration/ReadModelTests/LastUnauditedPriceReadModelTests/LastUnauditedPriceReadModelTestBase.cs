using System.Threading;
using EventFlow.Queries;
using Rob.ValuationMonitoring.Calculation.ReadModels;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.ReadModelTests.LastUnauditedPriceReadModelTests
{
    public abstract class LastUnauditedPriceReadModelTestBase : ValuationMonitoringSpecificationBase
    {
        protected UnauditedPrice Price { get; set; }

        protected string ValuationLineId { get; set; }

        protected LatestUnauditedPriceReadModel GetLastUnauditedPriceReadModel(string valuationLineId)
        {
            var queryProcessor = Resolver.Resolve<IQueryProcessor>();
            return queryProcessor.ProcessAsync(
                    new ReadModelByIdQuery<LatestUnauditedPriceReadModel>(valuationLineId),
                    CancellationToken.None)
                .GetAwaiter().GetResult();
        }
    }
}