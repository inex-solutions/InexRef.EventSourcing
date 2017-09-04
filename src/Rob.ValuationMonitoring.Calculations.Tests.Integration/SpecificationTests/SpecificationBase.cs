using NUnit.Framework;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests
{
    [TestFixture]
    public abstract class SpecificationBase
    {
        [OneTimeSetUp]
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

        [OneTimeTearDown]
        public void TearDown()
        {
            Cleanup();
        }

        protected virtual void Cleanup()
        {
        }
    }

    public abstract class SpecificationBase<TSubject> : SpecificationBase
    {
        protected TSubject Subject { get; set; }
    }
}
