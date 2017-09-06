<<<<<<< HEAD:src/Rob.EventSourcing.Tests/IntegrationTests/IntegrationTestHandlers.cs
﻿using Rob.EventSourcing.Persistence;
=======
﻿using System;
using Rob.ValuationMonitoring.EventSourcing.Persistence;
>>>>>>> 61efacf... Remove hard dependency on Guid as identifier:src/Rob.ValuationMonitoring.EventSourcing.Tests/IntegrationTests/IntegrationTestHandlers.cs

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class IntegrationTestHandlers
    {
        private readonly IAggregateRepository<AccountAggregateRoot, Guid> _repository;

        public IntegrationTestHandlers(IAggregateRepository<AccountAggregateRoot, Guid> repository)
        {
            _repository = repository;
        }

        public void Handle(AddAmountCommand command)
        {
            var item = _repository.GetOrCreateNew(command.Id);
            item.AddAmount(command.Amount);
            _repository.Save(item);
        }

        public void Handle(ResetBalanceCommand command)
        {
            var item = _repository.GetOrCreateNew(command.Id);
            item.ResetBalance();
            _repository.Save(item);
        }
    }
}