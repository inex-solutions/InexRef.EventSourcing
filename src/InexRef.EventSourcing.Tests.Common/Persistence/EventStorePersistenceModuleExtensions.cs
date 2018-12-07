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

using Autofac;
using InexRef.EventSourcing.Persistence.EventStore;
using InexRef.EventSourcing.Persistence.InMemory;
using InexRef.EventSourcing.Persistence.SqlServer;

namespace InexRef.EventSourcing.Tests.Common.Persistence
{
    public static class EventStorePersistenceModuleExtensions
    {
        public static void RegisterEventStorePersistenceModule(this ContainerBuilder containerBuilder, string persistenceProvider)
        {
            switch (persistenceProvider)
            {
                case "InMemory":
                    containerBuilder.RegisterModule<EventSourcingInMemoryPersistenceModule>();
                    break;
                case "SqlServer":
                    containerBuilder.RegisterModule<EventSourcingSqlServerPersistenceModule>();
                    break;
                case "EventStore":
                    containerBuilder.RegisterModule<EventStorePersistenceModule>();
                    break;
                default:
                    throw new TestSetupException($"Test setup failed. Persistence provider '{persistenceProvider}' not supported.");
            }
        }
    }
}