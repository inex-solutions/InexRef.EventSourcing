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
            AggregateId = Calculation.ValuationLineId.New;
            ValuationLineId = $"PORG-{DateTime.Now:yyyyMMddHHmmssfff}";
            Price = new UnauditedPrice(ValuationLineId, DateTime.Parse("01-Dec-2016"), "GBP", 12.3499M);
        }

        protected override async Task When() => await CommandBus.PublishAsync(new UpdateUnauditedPriceCommand(AggregateId, Price), CancellationToken.None).ConfigureAwait(false);

        [Then]
        public void a_read_model_is_created_for_the_valuation_line() => GetLastUnauditedPriceReadModel(ValuationLineId).ShouldNotBeNull();

        [Then]
        public void the_read_model_should_have_the_correct_price() => GetLastUnauditedPriceReadModel(ValuationLineId).UnauditedPrice.ShouldBe(Price.Value);

        [Then]
        public void the_read_model_should_have_the_correct_currency() => GetLastUnauditedPriceReadModel(ValuationLineId).Currency.ShouldBe(Price.Currency);

        [Then]
        public void the_read_model_should_have_the_correct_id() => GetLastUnauditedPriceReadModel(ValuationLineId).ValuationLineId.ShouldBe(ValuationLineId);

        [Then]
        public void the_read_model_should_have_the_correct_price_date_time() => GetLastUnauditedPriceReadModel(ValuationLineId).PriceDateTime.ShouldBe(Price.PriceDateTime);

        [Then]
        public void the_read_model_should_have_a_create_date_time() => GetLastUnauditedPriceReadModel(ValuationLineId).CreateTime.ShouldNotBeNull();
    }
}