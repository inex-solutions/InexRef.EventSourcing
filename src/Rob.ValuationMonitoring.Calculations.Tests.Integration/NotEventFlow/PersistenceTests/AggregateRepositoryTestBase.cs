using System;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Bus;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.PersistenceTests
{
    public abstract class AggregateRepositoryTestBase : SpecificationBase<IAggregateRepository<AccountAggregateRoot>>
    {
        protected Guid AggregateId { get; private set; }

        protected AccountAggregateRoot ReloadedAccountAggregateRoot { get; set; }

        protected override void SetUp()
        {
            AggregateId = Guid.NewGuid();
            Subject = new AggregateRepository<AccountAggregateRoot>(new InMemoryEventStore(new Bus()));
        }
    }
}