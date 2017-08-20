using NUnit.Framework;

namespace Rob.ValuationMonitoring.Calculations.Tests.Unit.SpecificationTests
{
    [TestFixture]
    public abstract class SpecificationBase
    {
        [SetUp]
        public void Init()
        {
            SetUp();
            Given();
            When();
        }

        protected virtual void SetUp()
        {    
        }

        protected abstract void When();

        protected abstract void Given();

        [TearDown]
        public void TearDown()
        {
            Cleanup();
        }

        protected virtual void Cleanup()
        {
        }
    }
}
