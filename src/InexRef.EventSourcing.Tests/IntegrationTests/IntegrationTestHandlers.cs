#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017 INEX Solutions Ltd
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
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.NaturalKey;

namespace InexRef.EventSourcing.Tests.IntegrationTests
{
    public class IntegrationTestHandlers : IHandle<AddAmountCommand>, IHandle<ResetBalanceCommand>
    {
        private readonly INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, string> _naturalKeyDrivenRepository;

        public IntegrationTestHandlers(INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, string> naturalKeyDrivenRepository)
        {
            _naturalKeyDrivenRepository = naturalKeyDrivenRepository;
        }

        public void Handle(AddAmountCommand command)
        {
            try
            {
                var item = _naturalKeyDrivenRepository.GetOrCreateNewByNaturalKey(
                    naturalKey: command.Id,
                    onCreateNew: newItem => newItem.InitialiseAccount(newItem.Id, command.Id));
                item.AddAmount(command.Amount);
                _naturalKeyDrivenRepository.Save(item);
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
                var item = _naturalKeyDrivenRepository.GetOrCreateNewByNaturalKey(
                    naturalKey: command.Id,
                    onCreateNew: newItem => newItem.InitialiseAccount(newItem.Id, command.Id));
                item.ResetBalance();
                _naturalKeyDrivenRepository.Save(item);
            }
            catch
            {
                Console.WriteLine($"Exception while handling {command}");
                throw;
            }
        }
    }
}