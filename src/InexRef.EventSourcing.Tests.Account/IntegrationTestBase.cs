﻿#region Copyright & License
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
using System.Linq;
using Autofac;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Tests.Account.Domain;
using InexRef.EventSourcing.Tests.Account.ReadModels;
using InexRef.EventSourcing.Tests.Common;
using InexRef.EventSourcing.Tests.Common.AccountDomain;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using NUnit.Framework;

namespace InexRef.EventSourcing.Tests.Account.DomainHost.Tests
{
    [TestFixture("EventStorePersistence=InMemory")]
    [TestFixture("EventStorePersistence=SqlServer")]
    public abstract class IntegrationTestBase : SpecificationBase<IBus>
    {
        private readonly IDictionary<string, string> _testFixtureOptions;

        protected string AccountId { get; private set; }

        protected INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, string> Repository { get; private set; }

        protected BalanceReadModel BalanceReadModel { get; private set; }

        protected IdGenerator IdGenerator { get; private set; }

        protected IntegrationTestBase(string testFixtureOptions)
        {
            _testFixtureOptions = testFixtureOptions
                .Split(',')
                .ToDictionary(item => item.Split('=')[0].Trim(), item => item.Split('=')[1].Trim());
        }

        protected override void SetUp()
        {
            IdGenerator = new IdGenerator("my-root");

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<EventSourcingCoreModule>();
            containerBuilder.RegisterModule<HandlerModule>();
            containerBuilder.RegisterEventStorePersistenceModule(_testFixtureOptions["EventStorePersistence"]);
            containerBuilder.RegisterModule<TestSetupModule>();

            containerBuilder.RegisterType<Calculator>().As<ICalculator>();
            containerBuilder.RegisterType<AccountAggregateRoot>();

            var container = containerBuilder.Build();

            Subject = container.Resolve<IBus>();
            Repository = container.Resolve<INaturalKeyDrivenAggregateRepository<AccountAggregateRoot, Guid, string>>();
            BalanceReadModel = container.Resolve<BalanceReadModel>();

            AccountId = IdGenerator.CreateAggregateId();
        }

        protected override void Cleanup()
        {
            Repository.DeleteByNaturalKey(AccountId);
        }
    }
}