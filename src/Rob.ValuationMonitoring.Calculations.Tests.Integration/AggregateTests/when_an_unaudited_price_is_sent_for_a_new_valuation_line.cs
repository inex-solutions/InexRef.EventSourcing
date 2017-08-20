using System;
using System.Threading;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.AggregateTests
{
    public class when_an_unaudited_price_is_sent_for_a_new_valuation_line : ValuationMonitoringSpecificationBase
    {
        private UnauditedPrice Price;

        protected override async Task Given()
        {
            AggregateId = Calculation.ValuationLineId.New;
            Price = new UnauditedPrice("PORG1", DateTime.Now, "GBP", 12.3499M);
        }

        protected override async Task When() => await CommandBus.PublishAsync(new UpdateUnauditedPriceCommand(AggregateId, Price), CancellationToken.None).ConfigureAwait(false);

        [Then]
        public void a_single_event_for_this_valuation_line_is_created_in_the_event_store() => GetEventsFromStore(AggregateId).Count.ShouldBe(1);

        [Then]
        public void the_valuation_line_should_report_the_price_as_its_latest_price() => GetAggregate(AggregateId).LastUnauditedPrice.ShouldBe(Price);
    }
}