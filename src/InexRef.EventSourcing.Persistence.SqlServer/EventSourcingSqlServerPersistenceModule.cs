#region Copyright & Licence
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
using InexRef.EventSourcing.Common.Container;
using InexRef.EventSourcing.Common.Hosting;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Persistence.SqlServer.NaturalKey;
using InexRef.EventSourcing.Persistence.SqlServer.Persistence;
using InexRef.EventSourcing.Persistence.SqlServer.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Persistence.SqlServer
{
    public class EventSourcingSqlServerPersistenceModule : ContainerConfigurationModule
    {
        protected override void Load(IServiceCollection containerBuilder)
        {
            containerBuilder.AddTransient<ISqlEventStoreJsonSerializer, SqlEventStoreJsonSerializer>();
            containerBuilder.AddSingleton(typeof(IEventStore<>), typeof(SqlEventStore<>));
            containerBuilder.AddSingleton(typeof(INaturalKeyToAggregateIdMap<,,>), typeof(SqlServerNaturalKeyToAggregateIdMap<,,>));

            containerBuilder.AddTransient<ISqlParameterCreator<string>, StringSqlParameterCreator>();
            containerBuilder.AddTransient<ISqlParameterCreator<Guid>, GuidSqlParameterCreator>();

            var sqlConfig = new SqlEventStoreConfiguration();
            HostedEnvironmentConfiguration.ConfigurationRoot.GetSection("SqlEventStore").Bind(sqlConfig);
            containerBuilder.AddSingleton(sqlConfig);
        }
    }
}