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
using InexRef.EventSourcing.Account.Contract.Public.Messages.Commands;
using InexRef.EventSourcing.Account.Contract.Public.Types;
using InexRef.EventSourcing.Account.Domain;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;

namespace InexRef.EventSourcing.Account.DomainHost
{
    public class AccountDomainHandlers : IHandle<CreateAccountCommand>, IHandle<AddAmountCommand>//, IHandle<ResetBalanceCommand>
    {
        private readonly INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, AccountId> _naturalKeyDrivenAggregateRepository;

        public AccountDomainHandlers(INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, AccountId> naturalKeyDrivenAggregateRepository)
        {
            _naturalKeyDrivenAggregateRepository = naturalKeyDrivenAggregateRepository;
        }

        public async Task Handle(CreateAccountCommand command)
        {
            try
            {
                var item = await _naturalKeyDrivenAggregateRepository.CreateNewByNaturalKey(
                    naturalKey: command.Id,
                    onCreateNew: async newItem => await newItem.InitialiseAccount(MessageMetadata.CreateFromMessage(command), newItem.Id,
                            command.Id));
                await _naturalKeyDrivenAggregateRepository.Save(item);
            }
            catch
            {
                Console.WriteLine($"Exception while handling {command}");
                throw;
            }
        }

        public async Task Handle(AddAmountCommand command)
        {
            try
            {
                var item = await _naturalKeyDrivenAggregateRepository.GetByNaturalKey(command.Id);
                await item.AddAmount(MessageMetadata.CreateFromMessage(command), command.Amount);
                await _naturalKeyDrivenAggregateRepository.Save(item);
            }
            catch
            {
                Console.WriteLine($"Exception while handling {command}");
                throw;
            }
        }

        //public async Task Handle(ResetBalanceCommand command)
        //{
        //    try
        //    {
        //        var item = await _naturalKeyDrivenAggregateRepository.GetByNaturalKey(command.Id);
        //        await item.ResetBalance(MessageMetadata.CreateFromMessage(command));
        //        await _naturalKeyDrivenAggregateRepository.Save(item);
        //    }
        //    catch
        //    {
        //        Console.WriteLine($"Exception while handling {command}");
        //        throw;
        //    }
        //}
    }
}