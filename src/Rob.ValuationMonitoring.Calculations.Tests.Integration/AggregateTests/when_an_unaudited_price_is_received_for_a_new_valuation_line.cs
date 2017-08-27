using System;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.AggregateTests
{
    public class when_an_unaudited_price_is_received_for_a_new_valuation_line : ValuationMonitoringSpecificationBase
    {
        private UpdateUnauditedPriceCommand UpdatePriceCommand;

        protected override async Task Given()
        {
            UpdatePriceCommand = new UpdateUnauditedPriceCommand(ValuationLineId, $"InitialName-{ValuationLineId}", DateTime.Now, "GBP", 12.3499M, DateTime.Now);
        }

        protected override async Task When() => await Publish(UpdatePriceCommand);

        [Then]
        public void the_valuation_line_should_report_the_valuation_line_name_correctly() => GetAggregate(ValuationLineId).ValuationLineName.ShouldBe(UpdatePriceCommand.Name);

        [Then]
        public void the_valuation_line_should_report_the_price_as_its_latest_price() => GetAggregate(ValuationLineId).LastUnauditedPrice.ShouldBe(UpdatePriceCommand.ToUnauditedPrice());
    }
}
