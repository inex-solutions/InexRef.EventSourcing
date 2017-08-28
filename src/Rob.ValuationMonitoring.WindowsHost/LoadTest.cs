using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.Commands;

namespace Rob.ValuationMonitoring.WindowsHost
{
    public class LoadTest : TestBase
    {

        public override async Task Execute()
        {
            var valuationLineId = CreateValuationLineId();

            Console.WriteLine("LoadTest start");
            var valuations = GenerateUpdateUnauditedPriceCommands(valuationLineId, DateTime.Parse("01-Jan-2006"), DateTime.Parse("01-Jan-2007"));

            foreach (var valuation in valuations)
            {
                await Publish(valuation);
                if (valuation.EffectiveDateTime.Day == 1)
                {
                    Console.WriteLine(valuation.EffectiveDateTime.ToShortDateString());
                }
            }

            var newValuation = new UpdateUnauditedPriceCommand(valuationLineId, $"Name-{valuationLineId}", DateTime.Parse("02-Jan-2016"), "GBP", 1, DateTime.Now);
            Console.WriteLine("Starting timed test");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await Publish(newValuation);

            TimeSpan elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Timed test completed in {elapsed.TotalMilliseconds}ms");
        }
    }
}
