using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Commands;
using EventFlow.Configuration;
using EventFlow.Core;
using EventFlow.EventStores;
using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.Commands;

namespace Rob.ValuationMonitoring.WindowsHost
{
    public abstract class TestBase : ITest
    {
        protected ICommandBus CommandBus { get; private set; }

        protected IEventStore EventStore { get; private set; }

        protected IRootResolver Resolver { get; private set; }

        public void Init(IRootResolver resolver)
        {
            Resolver = resolver;
            CommandBus = Resolver.Resolve<ICommandBus>();
            EventStore = Resolver.Resolve<IEventStore>();
        }

        public abstract Task Execute();

        protected ValuationLineId CreateValuationLineId()
        {
            var count = 1;
            return new ValuationLineId($"PORG-{DateTime.Now:yyyyMMddHHmmssfff}-{count}");
        }

        protected async Task Publish(ICommand<ValuationLineAggregate, ValuationLineId, ISourceId> command)
        {
            await CommandBus.PublishAsync(command, CancellationToken.None).ConfigureAwait(false);
        }


        protected static IEnumerable<UpdateUnauditedPriceCommand> GenerateUpdateUnauditedPriceCommands(
            ValuationLineId valuationLineId,
            DateTime startDate,
            DateTime endDate)
        {
            Console.WriteLine($"Generating daily unaudited values for every day between {startDate:dd MMMM yyyy} and {endDate:dd MMMM yyyy}");
            var currentDate = startDate;
            var startPrice = 1;
            while (currentDate <= endDate)
            {
                //   Console.WriteLine(currentDate.ToShortDateString());
                yield return new UpdateUnauditedPriceCommand(valuationLineId, $"Name-{valuationLineId}", currentDate, "GBP", startPrice++, DateTime.Now);
                currentDate = currentDate.AddDays(1);
            }
        }
    }
}