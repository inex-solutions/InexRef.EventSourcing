using Rob.EventSourcing.Tests.SpecificationTests;

namespace Rob.EventSourcing.Tests.AggregateTests
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
