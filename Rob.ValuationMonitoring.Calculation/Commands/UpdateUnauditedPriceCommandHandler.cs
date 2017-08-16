using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateUnauditedPriceCommandHandler : CommandHandler<ValuationLineAggregate, ValuationLineId, UpdateUnauditedPriceCommand>
    {
        public override Task ExecuteAsync(ValuationLineAggregate aggregate, UpdateUnauditedPriceCommand command, CancellationToken cancellationToken)
        {
            aggregate.UpdateUnauditedPrice(command.UnauditedPrice);
            return Task.FromResult(0);
        }
    }
}