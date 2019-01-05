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
using InexRef.EventSourcing.Account.Contract.Public.Types;
using InexRef.EventSourcing.Account.Domain;
using InexRef.EventSourcing.Account.ReadModels;
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Tests.Common;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace InexRef.EventSourcing.Account.DomainHost.Tests
{
    public abstract class AccountDomainTestBase : IntegrationTestBase<AccountAggregateRoot, AccountId>
    {
        protected AccountDomainTestBase(string hostingFlavour) : base(hostingFlavour)
        {
            NaturalId = AccountId.Parse(Guid.NewGuid().ToString());
        }

        protected BalanceReadModel BalanceReadModel { get; private set; }

        protected override void RegisterWithContainerBuilder(IServiceCollection containerBuilder)
        {
            containerBuilder.ConfigureFrom<AccountDomainHostModule>();
            containerBuilder.AddTransient<ICalculator, Calculator>();
            containerBuilder.AddTransient<AccountAggregateRoot>();
        }

        protected override void ResolveFromContainer(IServiceProvider container)
        {
            BalanceReadModel = container.GetRequiredService<BalanceReadModel>();
        }

        public void DirectlyReferenceNUnitToAidTestRunner()
        {
            Assert.IsTrue(true);
        }
    }
}