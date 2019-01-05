#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

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