using System;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.AggregateTests
{
    public class when_an_unaudited_price_is_received_for_an_existing_valuation_line_but_the_name_has_changed : ValuationMonitoringSpecificationBase
    {
        public UpdateUnauditedPriceCommand NewPriceCommand { get; set; }

        protected override async Task Given()
        {
            var initialPriceCommand = new UpdateUnauditedPriceCommand(ValuationLineId, $"InitialName-{ValuationLineId}", DateTime.Parse("01-Jan-2017"), "GBP", 15.0M, DateTime.Now);
            await Publish(initialPriceCommand);
            NewPriceCommand = new UpdateUnauditedPriceCommand(ValuationLineId, $"ChangedName-{ValuationLineId}", DateTime.Parse("02-Jan-2017"), "GBP", 15.0M, DateTime.Now);
        }

        protected override async Task When() => await Publish(NewPriceCommand);

        [Then]
        public void the_valuation_line_should_report_the_new_name_as_the_valuation_line_name() => GetAggregate(ValuationLineId).ValuationLineName.ShouldBe(NewPriceCommand.Name);

        [Then]
        public void the_valuation_line_should_report_the_new_price_as_its_latest_price() => GetAggregate(ValuationLineId).LastUnauditedPrice.ShouldBe(NewPriceCommand.ToUnauditedPrice());
    }
}