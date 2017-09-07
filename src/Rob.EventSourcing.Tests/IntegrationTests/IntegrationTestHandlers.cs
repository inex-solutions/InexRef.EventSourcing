

using System;
using Rob.EventSourcing.Persistence;

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