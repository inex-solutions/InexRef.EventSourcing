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

using System.IO;
using Autofac;
using InexRef.EventSourcing.Common;
using InexRef.EventSourcing.Persistence.EventStore;
using InexRef.EventSourcing.Persistence.SqlServer.Persistence;
using Microsoft.Extensions.Configuration;

namespace InexRef.EventSourcing.Tests.Common
{
    public class TestSetupModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile("config.xml", optional: false, reloadOnChange: false)
                .Build();

            var sqlConfig = new SqlEventStoreConfiguration();
            config.GetSection("SqlEventStore").Bind(sqlConfig);
            builder.RegisterInstance(sqlConfig).As<SqlEventStoreConfiguration>();

            var eventStoreConfiguration = new EventStoreConfiguration();
            config.GetSection("EventStore").Bind(eventStoreConfiguration);
            builder.RegisterInstance(eventStoreConfiguration).As<EventStoreConfiguration>();

            builder
                .RegisterType<DeterministicallyIncreasingDateTimeProvider>()
                .As<IDateTimeProvider>()
                .SingleInstance();
        }
    }
}