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

using Autofac;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Persistence.SqlServer.Utils;
using InexRef.EventSourcing.Tests.Account.Messages;
using InexRef.EventSourcing.Tests.Account.ReadModels;

namespace InexRef.EventSourcing.Tests.Account.DomainHost
{
    public class AccountDomainHostModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterType<BalanceReadModel>()
                .As<BalanceReadModel>()
                .As<IHandle<BalanceUpdatedEvent>>()
                .As<IHandle<AccountInitialisedEvent>>()
                .SingleInstance();

            containerBuilder
                .RegisterType<AccountDomainHandlers>()
                .As<IHandle<CreateAccountCommand>>()
                .As<IHandle<AddAmountCommand>>()
                .As<IHandle<ResetBalanceCommand>>();

            containerBuilder.RegisterType<AccountIdSqlParameterCreator>().As<ISqlParameterCreator<AccountId>>();
        }
    }
}