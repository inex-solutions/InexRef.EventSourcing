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
using InexRef.EventSourcing.Bus;
using InexRef.EventSourcing.Common;
using InexRef.EventSourcing.Common.Scoping;
using InexRef.EventSourcing.Common.SimpleInjector;
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Persistence;
using InexRef.EventSourcing.Domain;
using InexRef.EventSourcing.NaturalKey;
using InexRef.EventSourcing.Persistence.Common;
using SimpleInjector;

namespace InexRef.EventSourcing
{
    public class EventSourcingCoreModule : Module
    {
        protected override void Load(Container builder)
        {
            builder.Register<IBus, InMemoryBus>();
            builder.Register<IAggregateRootFactory, AggregateRootFactory>();
            builder.Register<IAggregateIdCreator<Guid>, GuidAggregateIdCreator>();
            builder.RegisterGeneric(typeof(AggregateRepository<,>)).As(typeof(IAggregateRepository<,>));
            builder.RegisterGeneric(typeof(NaturalKeyDrivenAggregateRepository<,,>)).As(typeof(INaturalKeyDrivenAggregateRepository<,,>));

            builder
                .Register<OperationContext>(container =>
                {
                    var operationScope = container.Resolve<IOperationScope>();
                    return new OperationContext(operationScope);
                })
                .As<IOperationContext>()
                .As<IOperationContextInternal>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<AutofacOperationScopeModule>();

            builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>();
        }
    }
}