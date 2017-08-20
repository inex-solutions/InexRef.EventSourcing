using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateAuditedPriceCommandHandler : CommandHandler<ValuationLineAggregate, ValuationLineId, UpdateAuditedPriceCommand>
    {
        public override Task ExecuteAsync(ValuationLineAggregate aggregate, UpdateAuditedPriceCommand command, CancellationToken cancellationToken)
        {
            aggregate.UpdateAuditedPrice(command.AuditedPrice);
            return Task.FromResult(0);
        }
    }
}