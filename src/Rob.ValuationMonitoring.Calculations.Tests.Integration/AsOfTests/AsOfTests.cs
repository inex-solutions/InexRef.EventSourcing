using System;
using System.Threading.Tasks;
using EventFlow.Queries;
using Rob.ValuationMonitoring.Calculation;
using Rob.ValuationMonitoring.Calculation.Commands;
using Rob.ValuationMonitoring.Calculation.ValueObjects;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Shouldly;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.AsOfTests
{
    public class AsOfTests : ValuationMonitoringSpecificationBase
    {
        protected override async Task Given()
        {
            var price = new UnauditedPrice(ValuationLineId, DateTime.Parse("11-Jan-2017"), "GBP", 11.0M, DateTime.Parse("11-Jan-2017"));
            await Publish(new UpdateUnauditedPriceCommand(AggregateId, price));
            price = new UnauditedPrice(ValuationLineId, DateTime.Parse("12-Jan-2017"), "GBP", 12.0M, DateTime.Parse("12-Jan-2017"));
            await Publish(new UpdateUnauditedPriceCommand(AggregateId, price));
            price = new UnauditedPrice(ValuationLineId, DateTime.Parse("13-Jan-2017"), "GBP", 13.0M, DateTime.Parse("13-Jan-2017"));
            await Publish(new UpdateUnauditedPriceCommand(AggregateId, price));
            price = new UnauditedPrice(ValuationLineId, DateTime.Parse("12-Jan-2017"), "GBP", 20.0M, DateTime.Parse("01-Mar-2017"));
            await Publish(new UpdateUnauditedPriceCommand(AggregateId, price));
        }

        protected override Task When()
        {
            return Task.FromResult(0);
        }

        [Then]
        public void the_price_on_12_Jan_asof_12_Jan_is_correct() => 
            AsOfQueries.GetUnauditedPrice(ValuationLineId, DateTime.Parse("12-Jan-2017"), asOfDateTime: DateTime.Parse("12-Jan-2017")).ShouldBe(12.0M);

        [Then]
        public void the_price_on_12_Jan_asof_01_Mar_is_correct() =>
            AsOfQueries.GetUnauditedPrice(ValuationLineId, DateTime.Parse("12-Jan-2017"), asOfDateTime: DateTime.Parse("01-Mar-2017")).ShouldBe(20.0M);
    }
}