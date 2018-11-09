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
using System.Collections.Generic;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.Account.Domain;
using InexRef.EventSourcing.Tests.Account.Messages;
using Microsoft.AspNetCore.Mvc;

namespace InexRef.EventSourcing.Tests.Account.Application.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, string> _repository;
        private readonly IBus _bus;

        public AccountController(
            INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, string> repository,
            IBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        [HttpGet("GetKeys")]
        public IEnumerable<string> GetKeys()
        {
            return _repository.GetAllKeys();
        }

        [HttpGet("Get")]
        public AccountAggregateRoot Get(string id)
        {
            return _repository.GetByNaturalKey(id);
        }

        [HttpPost("Create")]
        public void Create(string id)
        {
            _bus.Send(new CreateAccountCommand(MessageMetadata.CreateDefault(), AccountId.Parse(id)));
        }

        [HttpPost("AddAmount")]
        public void AddAmount(string id, decimal amount)
        {
            _bus.Send(new AddAmountCommand(MessageMetadata.CreateDefault(), AccountId.Parse(id), MonetaryAmount.Create(amount)));
        }

        [HttpPost("ResetBalance")]
        public void ResetBalance(string id)
        {
            _bus.Send(new ResetBalanceCommand(MessageMetadata.CreateDefault(), AccountId.Parse(id)));
        }
    }
}