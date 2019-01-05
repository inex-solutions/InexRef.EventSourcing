using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace InexRef.EventSourcing.Tests.Common.SpecificationFramework
{
    [SuppressMessage("NDepend", "ND2102:AvoidDefiningMultipleTypesInASourceFile")]
    [TestFixture]
    public abstract class SpecificationBase
    {
        protected Exception CaughtException { get; set; }

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

        protected virtual void When() { }

        protected virtual void Given() { }

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

        protected override void Cleanup()
        {
            base.Cleanup();
            var disposable = Subject as IDisposable;
            disposable?.Dispose();
        }
    }
}