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
using InexRef.EventSourcing.Common.Scoping;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.Account.Domain;
using InexRef.EventSourcing.Tests.Account.Messages;

namespace InexRef.EventSourcing.Tests.Account.DomainHost
{


    public class AccountDomainHandlers : IHandle<AddAmountCommand>, IHandle<ResetBalanceCommand>
    {
        private IOperationScopeManager OperationScopeManager { get; }

        public AccountDomainHandlers(IOperationScopeManager operationScopeManager)
        {
            OperationScopeManager = operationScopeManager;
        }

        public void Handle(AddAmountCommand command)
        {
            try
            {
                using (var operationScope = OperationScopeManager.CreateScopeFromMessage(command))
                {
                    var naturalKeyDrivenRepository = operationScope.Get< INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, string> >();
                    var item = naturalKeyDrivenRepository.GetOrCreateNewByNaturalKey(
                        naturalKey: command.Id,
                        onCreateNew: newItem => newItem.InitialiseAccount(MessageMetadata.CreateFromMessage(command), newItem.Id, command.Id));
                    item.AddAmount(MessageMetadata.CreateFromMessage(command), command.Amount);
                    naturalKeyDrivenRepository.Save(item);
                }
            }
            catch
            {
                Console.WriteLine($"Exception while handling {command}");
                throw;
            }
        }

        public void Handle(ResetBalanceCommand command)
        {
            try
            {
                using (var operationScope = OperationScopeManager.CreateScopeFromMessage(command))
                {
                    var naturalKeyDrivenRepository = operationScope.Get<INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, string>>();
                    var item = naturalKeyDrivenRepository.GetOrCreateNewByNaturalKey(
                        naturalKey: command.Id,
                        onCreateNew: newItem => newItem.InitialiseAccount(MessageMetadata.CreateFromMessage(command), newItem.Id, command.Id));
                    item.ResetBalance(MessageMetadata.CreateFromMessage(command));
                    naturalKeyDrivenRepository.Save(item);
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