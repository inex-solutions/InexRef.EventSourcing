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
using Autofac;
using InexRef.EventSourcing.Account.Contract.Public.Types;
using InexRef.EventSourcing.Account.Domain;
using InexRef.EventSourcing.Account.ReadModels;
using InexRef.EventSourcing.Tests.Common;
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

        protected override void RegisterWithContainerBuilder(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<AccountDomainHostModule>();
            containerBuilder.RegisterType<Calculator>().As<ICalculator>();
            containerBuilder.RegisterType<AccountAggregateRoot>();
        }

        protected override void ResolveFromContainer(IContainer container)
        {
            BalanceReadModel = container.Resolve<BalanceReadModel>();
        }

        public void DirectlyReferenceNUnitToAidTestRunner()
        {
            Assert.IsTrue(true);
        }
    }
}