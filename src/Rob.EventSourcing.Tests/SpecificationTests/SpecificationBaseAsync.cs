using System.Threading.Tasks;
using NUnit.Framework;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests
{
    [TestFixture]
    public abstract class SpecificationBaseAsync
    {
        [OneTimeSetUp]
        public async Task Init()
        {
            SetUp();
            await Given();
            await When();
        }

        protected virtual void SetUp()
        {
        }

        protected abstract Task When();

        protected abstract Task Given();

        [OneTimeTearDown]
        public void TearDown()
        {
            Cleanup();
        }

        protected virtual void Cleanup()
        {
        }
    }
}