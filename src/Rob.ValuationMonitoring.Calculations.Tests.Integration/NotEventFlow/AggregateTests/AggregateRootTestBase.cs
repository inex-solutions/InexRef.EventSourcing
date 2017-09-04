using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.AggregateTests
{
    public abstract class AggregateRootTestBase<TAggregateRoot> : SpecificationBase where TAggregateRoot : new() 
    {
        protected TAggregateRoot Subject { get; set; }

        protected override void SetUp()
        {
            Subject = new TAggregateRoot();
        }
    }
}
