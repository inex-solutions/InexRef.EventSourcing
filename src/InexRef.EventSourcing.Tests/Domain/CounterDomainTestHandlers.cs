using System;
using System.Threading.Tasks;
using InexRef.EventSourcing.Common;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.OperationContext;
using InexRef.EventSourcing.Contracts.Persistence;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class CounterDomainTestHandlers : IHandle<InitialiseCounterCommand>, IHandle<IncrementCounterCommand>
    {
        private readonly INaturalKeyDrivenAggregateRepository<CounterAggregateRoot, Guid, string> _naturalKeyDrivenRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly IUpdateOperationContext _operationContextUpdater;

        public CounterDomainTestHandlers(
            IUpdateOperationContext operationContextUpdater,
            INaturalKeyDrivenAggregateRepository<CounterAggregateRoot, Guid, string> naturalKeyDrivenRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _naturalKeyDrivenRepository = naturalKeyDrivenRepository;
            _dateTimeProvider = dateTimeProvider;
            _operationContextUpdater = operationContextUpdater;
        }

        public async Task Handle(InitialiseCounterCommand command)
        {
            try
            {
                _operationContextUpdater.SetMessageMetadataOnOperationContext(command.MessageMetadata.SourceCorrelationId, _dateTimeProvider.GetUtcNow());
                var item = await _naturalKeyDrivenRepository.GetOrCreateNewByNaturalKey(
                    naturalKey: command.Id,
                    onCreateNew: async newItem => await newItem.Initialise(newItem.Id));
                await _naturalKeyDrivenRepository.Save(item);
            }
            catch
            {
                Console.WriteLine($"Exception while handling {command}");
                throw;
            }
        }

        public async Task Handle(IncrementCounterCommand command)
        {
            try
            {
                _operationContextUpdater.SetMessageMetadataOnOperationContext(command.MessageMetadata.SourceCorrelationId, _dateTimeProvider.GetUtcNow());
                var item = await _naturalKeyDrivenRepository.GetByNaturalKey(command.Id);
                await item.Increment();
                await _naturalKeyDrivenRepository.Save(item);
            }
            catch
            {
                Console.WriteLine($"Exception while handling {command}");
                throw;
            }
        }
    }
}