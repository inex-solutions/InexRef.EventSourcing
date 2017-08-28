using System;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ReadModels;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.ReadModelTests.LastUnauditedPriceReadModelTests
{
    public class when_an_unaudited_price_with_a_newer_price_date_and_changed_name_is_received : LastUnauditedPriceReadModelTestBase
    {
        private LatestUnauditedPriceReadModel OriginalReadModel;

        protected override async Task Given()
        {
            UpdateUnauditedPriceCommand = new UpdateUnauditedPriceCommand(ValuationLineId, $"InitialName-{ValuationLineId}", DateTime.Parse("01-Jan-2017"), "GBP", 10.0M, DateTime.Now);
            await Publish(UpdateUnauditedPriceCommand);
            OriginalReadModel = LatestUnauditedPriceReadModel;

            UpdateUnauditedPriceCommand = new UpdateUnauditedPriceCommand(ValuationLineId, $"ChangedName-{ValuationLineId}", DateTime.Parse("02-Jan-2017"), "GBP", 15.0M, DateTime.Now);
        }

        protected override async Task When() => await Publish(UpdateUnauditedPriceCommand);

        [Then]
        public void the_read_model_should_have_the_newer_valuation_line_name() => LatestUnauditedPriceReadModel.ValuationLineName.ShouldBe(UpdateUnauditedPriceCommand.Name);

        [Then]
        public void the_read_model_should_have_the_newer_price() => LatestUnauditedPriceReadModel.UnauditedPrice.ShouldBe(UpdateUnauditedPriceCommand.Value);

        [Then]
        public void the_read_model_should_have_the_correct_currency() => LatestUnauditedPriceReadModel.Currency.ShouldBe(UpdateUnauditedPriceCommand.Currency);

        [Then]
        public void the_read_model_should_have_the_correct_id() => LatestUnauditedPriceReadModel.ValuationLineId.ShouldBe(ValuationLineId.Value);

        [Then]
        public void the_read_model_should_have_the_newer_effective_date_time() => LatestUnauditedPriceReadModel.EffectiveDateTime.ShouldBe(UpdateUnauditedPriceCommand.EffectiveDateTime);

        [Then]
        public void the_read_model_should_have_the_original_create_date_time() => LatestUnauditedPriceReadModel.CreateTime.ShouldBe(OriginalReadModel.CreateTime);

        [Then]
        public void the_read_model_should_have_a_higher_sequence_number_than_the_original() => LatestUnauditedPriceReadModel.SequenceNumber.ShouldBeGreaterThan(OriginalReadModel.SequenceNumber);
    }
}