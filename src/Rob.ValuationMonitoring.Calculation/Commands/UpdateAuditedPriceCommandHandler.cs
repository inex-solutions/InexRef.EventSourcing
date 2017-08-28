﻿using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;

namespace Rob.ValuationMonitoring.Calculation.Commands
{
    public class UpdateAuditedPriceCommandHandler : CommandHandler<ValuationLineAggregate, ValuationLineId, UpdateAuditedPriceCommand>
    {
        public override Task ExecuteAsync(ValuationLineAggregate aggregate, UpdateAuditedPriceCommand command, CancellationToken cancellationToken)
        {
            if (aggregate.IsNew
                || aggregate.ValuationLineName != command.Name)
            {
                aggregate.UpdateValuationLineName(command.Name, command.EffectiveDateTime);
            }

            aggregate.UpdateAuditedPrice(new AuditedPrice(command.EffectiveDateTime, command.Currency, command.Value, command.AsOfDateTime));

            return Task.FromResult(0);
        }
    }
}