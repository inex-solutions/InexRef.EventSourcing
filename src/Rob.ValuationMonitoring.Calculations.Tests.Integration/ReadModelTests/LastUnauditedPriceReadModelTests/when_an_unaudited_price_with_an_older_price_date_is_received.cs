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
            AggregateId = Calculation.ValuationLineId.New;
            ValuationLineId = $"PORG-{DateTime.Now:yyyyMMddHHmmssfff}";
            Price = new UnauditedPrice(ValuationLineId, DateTime.Parse("01-Jan-2017"), "GBP", 10.0M);
            await CommandBus.PublishAsync(new UpdateUnauditedPriceCommand(AggregateId, Price), CancellationToken.None).ConfigureAwait(false);

            OriginalReadModel = GetLastUnauditedPriceReadModel(ValuationLineId);

            Price = new UnauditedPrice(ValuationLineId, DateTime.Parse("31-Dec-2016"), "GBP", 15.0M);
        }

        protected override async Task When() => await CommandBus.PublishAsync(new UpdateUnauditedPriceCommand(AggregateId, Price), CancellationToken.None).ConfigureAwait(false);

        [Then]
        public void the_read_model_should_have_the_original_price() => GetLastUnauditedPriceReadModel(ValuationLineId).UnauditedPrice.ShouldBe(OriginalReadModel.UnauditedPrice);

        [Then]
        public void the_read_model_should_have_the_correct_currency() => GetLastUnauditedPriceReadModel(ValuationLineId).Currency.ShouldBe(Price.Currency);

        [Then]
        public void the_read_model_should_have_the_correct_id() => GetLastUnauditedPriceReadModel(ValuationLineId).ValuationLineId.ShouldBe(ValuationLineId);

        [Then]
        public void the_read_model_should_have_the_original_price_date_time() => GetLastUnauditedPriceReadModel(ValuationLineId).PriceDateTime.ShouldBe(OriginalReadModel.PriceDateTime);

        [Then]
        public void the_read_model_should_have_the_original_create_date_time() => GetLastUnauditedPriceReadModel(ValuationLineId).CreateTime.ShouldBe(OriginalReadModel.CreateTime);

        [Then]
        public void the_read_model_should_have_the_same_sequence_number_as_the_original() => GetLastUnauditedPriceReadModel(ValuationLineId).SequenceNumber.ShouldBe(OriginalReadModel.SequenceNumber);
    }
}