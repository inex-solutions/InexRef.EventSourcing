using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;
using Rob.ValuationMonitoring.EventSourcing.Persistence;

namespace Rob.ValuationMonitoring.EventSourcing.Tests.PersistenceTests
{
    public abstract class AggregateRepositoryTestBase : SpecificationBase<IAggregateRepository<AccountAggregateRoot>>
    {
        protected Guid AggregateId { get; private set; }

        protected AccountAggregateRoot ReloadedAccountAggregateRoot { get; set; }

        protected override void SetUp()
        {
            AggregateId = Guid.NewGuid();
            Subject = new AggregateRepository<AccountAggregateRoot>(new InMemoryEventStore(new Bus.Bus()));
        }
    }
}