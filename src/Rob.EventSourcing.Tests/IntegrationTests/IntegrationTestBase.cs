using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Rob.ValuationMonitoring.EventSourcing.Bus;
using Rob.ValuationMonitoring.EventSourcing.Persistence;

namespace Rob.ValuationMonitoring.EventSourcing.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase : SpecificationBase<IBus>
    {
        protected Guid AggregateId { get; private set; }

        protected IAggregateRepository<AccountAggregateRoot> Repository { get; private set; }

        protected override void SetUp()
        {
            Subject = new Bus.Bus();
            AggregateId = Guid.NewGuid();
            Repository = new AggregateRepository<AccountAggregateRoot>(new InMemoryEventStore(Subject));
            var handlers = new IntegrationTestHandlers(Repository);
            Subject.RegisterHandler<AddAmountCommand>(handlers.Handle);
            Subject.RegisterHandler<ResetBalanceCommand>(handlers.Handle);
        }
    }
}
