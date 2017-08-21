using System;
using System.Threading;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.AggregateTests
{
    public class when_an_unaudited_price_is_received_for_an_existing_valuation_line : ValuationMonitoringSpecificationBase
    {
        private UnauditedPrice NewPrice;

        protected override async Task Given()
        {
            var valuationLineId = CreateValuationLineId();
            AggregateId = Calculation.ValuationLineId.New;
            var oldPrice = new UnauditedPrice(valuationLineId, DateTime.Now, "GBP", 12.3499M);
            await Publish(new UpdateUnauditedPriceCommand(AggregateId, oldPrice));
            NewPrice = new UnauditedPrice(valuationLineId, DateTime.Now, "GBP", 15.00M);
        }

        protected override async Task When() => await Publish(new UpdateUnauditedPriceCommand(AggregateId, NewPrice));

        [Then]
        public void two_events_for_this_valuation_line_are_now_present_in_the_event_store() => GetEventsFromStore(AggregateId).Count.ShouldBe(2);

        [Then]
        public void the_valuation_line_should_report_the_new_price_as_its_latest_price() => GetAggregate(AggregateId).LastUnauditedPrice.ShouldBe(NewPrice);
    }
}