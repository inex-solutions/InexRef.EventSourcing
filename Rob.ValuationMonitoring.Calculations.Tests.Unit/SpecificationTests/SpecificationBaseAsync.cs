using System.Threading.Tasks;
using NUnit.Framework;

namespace Rob.ValuationMonitoring.Calculations.Tests.Unit.SpecificationTests
{
    [TestFixture]
    public abstract class SpecificationBaseAsync
    {
        [SetUp]
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