using System;
using System.Threading;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.AggregateTests
{
    public class when_an_audited_price_is_received_for_an_existing_valuation_line : ValuationMonitoringSpecificationBase
    {
        private AuditedPrice AuditedPrice;
        private UnauditedPrice OldPrice;
       
        protected override async Task Given()
        {
            OldPrice = new UnauditedPrice(ValuationLineId, DateTime.Parse("01-Jan-2017"), "GBP", 8.5M);
            await Publish(new UpdateUnauditedPriceCommand(AggregateId, OldPrice));
            AuditedPrice = new AuditedPrice(ValuationLineId, DateTime.Parse("31-Dec-2016"), "GBP", 10.0M);
        }

        protected override async Task When() => await Publish(new UpdateAuditedPriceCommand(AggregateId, AuditedPrice));

        [Then]
        public void two_events_for_this_valuation_line_are_now_present_in_the_event_store() => GetEventsFromStore(AggregateId).Count.ShouldBe(2);

        [Then]
        public void the_valuation_line_should_report_the_audited_price_as_its_reference_price() => GetAggregate(AggregateId).ReferencePrice.ShouldBe(AuditedPrice);

        [Then]
        public void the_valuation_line_should_report_the_old_price_as_its_latest_unaudited_price() => GetAggregate(AggregateId).LastUnauditedPrice.ShouldBe(OldPrice);

        [Then]
        public void the_valuation_line_should_report_its_valuation_change_correctly() => GetAggregate(AggregateId).ValuationChange.ShouldBe(-0.15M);
    }
}