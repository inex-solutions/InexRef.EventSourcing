using System;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.AggregateTests
{
    public class when_an_audited_price_is_received_for_an_existing_valuation_line : ValuationMonitoringSpecificationBase
    {
        private UpdateAuditedPriceCommand AuditedPriceCommand;
        private UpdateUnauditedPriceCommand OldPriceCommand;
       
        protected override async Task Given()
        {
            OldPriceCommand = new UpdateUnauditedPriceCommand(ValuationLineId, $"InitialName-{ValuationLineId}", DateTime.Parse("01-Jan-2017"), "GBP", 8.5M, DateTime.Now);
            await Publish(OldPriceCommand);
            AuditedPriceCommand = new UpdateAuditedPriceCommand(ValuationLineId, $"InitialName-{ValuationLineId}", DateTime.Parse("31-Dec-2016"), "GBP", 10.0M, DateTime.Now);
        }

        protected override async Task When() => await Publish(AuditedPriceCommand);

        [Then]
        public void two_events_for_this_valuation_line_are_now_present_in_the_event_store() => GetEventsFromStore(ValuationLineId).Count.ShouldBe(2);

        [Then]
        public void the_valuation_line_should_report_the_audited_price_as_its_reference_price() => GetAggregate(ValuationLineId).ReferencePrice.ShouldBe(AuditedPriceCommand.ToAuditedPrice());

        [Then]
        public void the_valuation_line_should_report_the_old_price_as_its_latest_unaudited_price() => GetAggregate(ValuationLineId).LastUnauditedPrice.ShouldBe(OldPriceCommand.ToUnauditedPrice());

        [Then]
        public void the_valuation_line_should_report_its_valuation_change_correctly() => GetAggregate(ValuationLineId).ValuationChange.ShouldBe(-0.15M);
    }
}