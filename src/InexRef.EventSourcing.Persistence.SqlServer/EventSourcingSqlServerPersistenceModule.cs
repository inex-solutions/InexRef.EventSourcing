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
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Persistence.SqlServer.NaturalKey;
using InexRef.EventSourcing.Persistence.SqlServer.Persistence;

namespace InexRef.EventSourcing.Persistence.SqlServer
{
    public class EventSourcingSqlServerPersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlEventStoreJsonSerializer>().As<ISqlEventStoreJsonSerializer>();
            builder.RegisterGeneric(typeof(SqlEventStore<>)).As(typeof(IEventStore<>)).SingleInstance();
            builder.RegisterGeneric(typeof(SqlServerNaturalKeyToAggregateIdMap<,,>)).As(typeof(INaturalKeyToAggregateIdMap<,,>)).SingleInstance();
        }
    }
}