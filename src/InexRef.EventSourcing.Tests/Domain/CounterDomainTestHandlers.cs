#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2018 INEX Solutions Ltd
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
using InexRef.EventSourcing.Common.Scoping;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class CounterDomainTestHandlers : IHandle<InitialiseCounterCommand>, IHandle<IncrementCounterCommand>
    {
        private IOperationScopeManager OperationScopeManager { get; }

        public CounterDomainTestHandlers(IOperationScopeManager operationScopeManager)
        {
            OperationScopeManager = operationScopeManager;
        }

        public async Task Handle(InitialiseCounterCommand command)
        {
            try
            {
                using (var operationScope = OperationScopeManager.CreateScopeFromMessage(command))
                {
                    var naturalKeyDrivenRepository = operationScope.Get<INaturalKeyDrivenAggregateRepository<CounterAggregateRoot, Guid, string>>();
                    var item = await naturalKeyDrivenRepository.GetOrCreateNewByNaturalKey(
                        naturalKey: command.Id,
                        onCreateNew: async newItem => await newItem.Initialise(newItem.Id));
                    await naturalKeyDrivenRepository.Save(item);
                }
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
                using (var operationScope = OperationScopeManager.CreateScopeFromMessage(command))
                {
                    var naturalKeyDrivenRepository = operationScope.Get<INaturalKeyDrivenAggregateRepository<CounterAggregateRoot, Guid, string>>();
                    var item = await naturalKeyDrivenRepository.GetByNaturalKey(command.Id);
                    await item.Increment();
                    await naturalKeyDrivenRepository.Save(item);
                }
            }
            catch
            {
                Console.WriteLine($"Exception while handling {command}");
                throw;
            }
        }
    }
}