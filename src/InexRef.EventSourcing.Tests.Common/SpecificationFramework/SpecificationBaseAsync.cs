using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace InexRef.EventSourcing.Tests.Common.SpecificationFramework
{
    [TestFixture]
    public abstract class SpecificationBaseAsync
    {
        protected Exception CaughtException { get; set; }

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

        protected virtual async Task When() => await Task.CompletedTask;

        protected virtual async Task Given() => await Task.CompletedTask;

        [OneTimeTearDown]
        public void TearDown()
        {
            Cleanup();
        }

        protected virtual void Cleanup()
        {
        }
    }

    public abstract class SpecificationBaseAsync<TSubject> : SpecificationBaseAsync
    {
        protected TSubject Subject { get; set; }

        protected override void Cleanup()
        {
            base.Cleanup();
            var disposable = Subject as IDisposable;
            disposable?.Dispose();
        }
    }
}