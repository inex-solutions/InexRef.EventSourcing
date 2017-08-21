using System;
using System.Linq;
using System.Threading;
using EventFlow.Configuration;
using EventFlow.Queries;
using Rob.ValuationMonitoring.Calculation.ReadModels;

namespace Rob.ValuationMonitoring.Calculation
{
    public class AsOfQueries
    {
        private readonly IRootResolver _resolver;

        public AsOfQueries(IRootResolver resolver)
        {
            _resolver = resolver;
        }

        public decimal GetUnauditedPrice(string valuationLineId, DateTime priceDateTime, DateTime asOfDateTime)
        {
            var queryProcessor = _resolver.Resolve<IQueryProcessor>();
            var rawEventReadModel = queryProcessor.ProcessAsync(
                    new ReadModelByIdQuery<RawEventReadModel>(valuationLineId),
                    CancellationToken.None)
                .GetAwaiter().GetResult();

            var events = rawEventReadModel.GetRawEventsAsOf(asOfDateTime);

            var result = events
                .Where(e => e.UnauditedPrice.AsOfDateTime <= asOfDateTime)
                .OrderByDescending(e => e.UnauditedPrice.AsOfDateTime)
                .FirstOrDefault(e => e.UnauditedPrice.PriceDateTime == priceDateTime);

            return result.UnauditedPrice.Value;
        }
    }
}