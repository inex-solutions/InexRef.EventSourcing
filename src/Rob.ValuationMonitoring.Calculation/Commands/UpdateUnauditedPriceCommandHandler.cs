using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateUnauditedPriceCommandHandler : CommandHandler<ValuationLineAggregate, ValuationLineId, UpdateUnauditedPriceCommand>
    {
        public override Task ExecuteAsync(ValuationLineAggregate aggregate, UpdateUnauditedPriceCommand command, CancellationToken cancellationToken)
        {
            if (aggregate.IsNew 
                || aggregate.ValuationLineName != command.Name)
            {
                aggregate.UpdateValuationLineName(command.Name, command.EffectiveDateTime);
            }

            aggregate.UpdateUnauditedPrice(new UnauditedPrice(command.EffectiveDateTime, command.Currency, command.Value, command.AsOfDateTime));

            return Task.FromResult(0);
        }
    }
}