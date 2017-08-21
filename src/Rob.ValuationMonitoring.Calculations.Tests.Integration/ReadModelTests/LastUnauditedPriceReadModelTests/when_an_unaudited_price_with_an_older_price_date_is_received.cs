using System;
using System.Threading;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ReadModels;
using Rob.ValuationMonitoring.Calculation.ValueObjects;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.ReadModelTests.LastUnauditedPriceReadModelTests
{
    public class when_an_unaudited_price_with_an_older_price_date_is_received : LastUnauditedPriceReadModelTestBase
    {
        private LatestUnauditedPriceReadModel OriginalReadModel;

        protected override async Task Given()
        {
            Price = new UnauditedPrice(ValuationLineId, DateTime.Parse("01-Jan-2017"), "GBP", 10.0M);
            await Publish(new UpdateUnauditedPriceCommand(AggregateId, Price));
            OriginalReadModel = LatestUnauditedPriceReadModel;

            Price = new UnauditedPrice(ValuationLineId, DateTime.Parse("31-Dec-2016"), "GBP", 15.0M);
        }

        protected override async Task When() => await Publish(new UpdateUnauditedPriceCommand(AggregateId, Price));

        [Then]
        public void the_read_model_should_have_the_original_price() => LatestUnauditedPriceReadModel.UnauditedPrice.ShouldBe(OriginalReadModel.UnauditedPrice);

        [Then]
        public void the_read_model_should_have_the_correct_currency() => LatestUnauditedPriceReadModel.Currency.ShouldBe(Price.Currency);

        [Then]
        public void the_read_model_should_have_the_correct_id() => LatestUnauditedPriceReadModel.ValuationLineId.ShouldBe(ValuationLineId);

        [Then]
        public void the_read_model_should_have_the_original_price_date_time() => LatestUnauditedPriceReadModel.PriceDateTime.ShouldBe(OriginalReadModel.PriceDateTime);

        [Then]
        public void the_read_model_should_have_the_original_create_date_time() => LatestUnauditedPriceReadModel.CreateTime.ShouldBe(OriginalReadModel.CreateTime);

        [Then]
        public void the_read_model_should_have_the_same_sequence_number_as_the_original() => LatestUnauditedPriceReadModel.SequenceNumber.ShouldBe(OriginalReadModel.SequenceNumber);
    }
}