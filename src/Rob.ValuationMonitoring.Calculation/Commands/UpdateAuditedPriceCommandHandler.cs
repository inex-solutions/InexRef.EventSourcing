using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateAuditedPriceCommandHandler : CommandHandler<ValuationLineAggregate, ValuationLineId, UpdateAuditedPriceCommand>
    {
        public override Task ExecuteAsync(ValuationLineAggregate aggregate, UpdateAuditedPriceCommand command, CancellationToken cancellationToken)
        {
            aggregate.UpdateAuditedPrice(new AuditedPrice(command.Id, command.PriceDateTime, command.Currency, command.Value, command.AsOfDateTime));
            return Task.FromResult(0);
        }
    }
}