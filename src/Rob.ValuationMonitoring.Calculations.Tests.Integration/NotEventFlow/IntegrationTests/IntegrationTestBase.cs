using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Bus;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.IntegrationTests
{
    public abstract class IntegrationTestBase : SpecificationBase<IBus>
    {
        protected Guid AggregateId { get; private set; }

        protected IAggregateRepository<AccountAggregateRoot> Repository { get; private set; }

        protected override void SetUp()
        {
            AggregateId = Guid.NewGuid();
            Repository = new AggregateRepository<AccountAggregateRoot>(new InMemoryEventStore());
            var handlers = new IntegrationTestHandlers(Repository);
            Subject = new Bus();
            Subject.RegisterHandler<AddAmountCommand>(handlers.Handle);
            Subject.RegisterHandler<ResetBalanceCommand>(handlers.Handle);
        }
    }
}
