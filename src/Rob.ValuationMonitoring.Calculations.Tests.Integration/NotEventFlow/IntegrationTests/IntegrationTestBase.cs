using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rob.ValuationMonitoring.Calculation.NotEventFlow.Persistence;
using Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.NotEventFlow.IntegrationTests
{
    public abstract class IntegrationTestBase : SpecificationBase<IntegrationTestHandlers>
    {
        protected Guid AggregateId { get; private set; }

        protected IAggregateRepository<AccountAggregateRoot> Repository { get; private set; }

        protected AccountAggregateRoot ReloadedAccountAggregateRoot { get; set; }

        protected override void SetUp()
        {
            AggregateId = Guid.NewGuid();
            Repository = new AggregateRepository<AccountAggregateRoot>(new InMemoryEventStore());
            Subject = new IntegrationTestHandlers(Repository);
        }
    }
}
