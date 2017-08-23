using System;
using System.Threading;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.ReadModelTests.LastUnauditedPriceReadModelTests
{
    public class when_an_unaudited_price_is_received_for_an_unknown_line : LastUnauditedPriceReadModelTestBase
    {
        protected override async Task Given()
        {
            Price = new UnauditedPrice(DateTime.Parse("01-Dec-2016"), "GBP", 12.3499M, DateTime.Now);
        }

        protected override async Task When() => await Publish(Price.ToUpdateUnauditedPriceCommand(ValuationLineId));

        [Then]
        public void a_read_model_is_created_for_the_valuation_line() => LatestUnauditedPriceReadModel.ShouldNotBeNull();

        [Then]
        public void the_read_model_should_have_the_correct_price() => LatestUnauditedPriceReadModel.UnauditedPrice.ShouldBe(Price.Value);

        [Then]
        public void the_read_model_should_have_the_correct_currency() => LatestUnauditedPriceReadModel.Currency.ShouldBe(Price.Currency);

        [Then]
        public void the_read_model_should_have_the_correct_id() => LatestUnauditedPriceReadModel.ValuationLineId.ShouldBe(ValuationLineId.Value);

        [Then]
        public void the_read_model_should_have_the_correct_price_date_time() => LatestUnauditedPriceReadModel.PriceDateTime.ShouldBe(Price.PriceDateTime);

        [Then]
        public void the_read_model_should_have_a_create_date_time() => LatestUnauditedPriceReadModel.CreateTime.ShouldNotBeNull();
    }
}
