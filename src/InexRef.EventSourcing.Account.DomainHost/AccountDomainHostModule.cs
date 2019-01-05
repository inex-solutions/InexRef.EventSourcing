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
using InexRef.EventSourcing.Account.Contract.Public.Messages;
using InexRef.EventSourcing.Account.Contract.Public.Types;
using InexRef.EventSourcing.Account.ReadModels;
using InexRef.EventSourcing.Common.Container;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Account.DomainHost
{
    public class AccountDomainHostModule : ContainerConfigurationModule
    {
        protected override void Load(IServiceCollection containerBuilder)
        {
            containerBuilder
                .AddSingleton<BalanceReadModel>()
                .AddSingleton<IHandle<BalanceUpdatedEvent>>(provider => provider.GetRequiredService<BalanceReadModel>())
                .AddSingleton<IHandle<AccountInitialisedEvent>>(provider => provider.GetRequiredService<BalanceReadModel>());

            containerBuilder
                .AddScoped<AccountDomainHandlers>()
                .AddScoped<IHandle<CreateAccountCommand>>(provider => provider.GetRequiredService<AccountDomainHandlers>())
                .AddScoped<IHandle<AddAmountCommand>>(provider => provider.GetRequiredService<AccountDomainHandlers>())
                .AddScoped<IHandle<ResetBalanceCommand>>(provider => provider.GetRequiredService<AccountDomainHandlers>());

            containerBuilder.AddTransient<ISqlParameterCreator<AccountId>, AccountIdSqlParameterCreator>();
        }
    }
}