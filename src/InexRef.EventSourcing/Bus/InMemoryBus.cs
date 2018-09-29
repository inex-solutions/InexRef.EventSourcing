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

using System.Collections;
using System.Collections.Generic;
using Autofac;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Utils.Dynamic;

namespace InexRef.EventSourcing.Bus
{
    public class InMemoryBus : IBus
    {
        private readonly IComponentContext _componentContext;

        public InMemoryBus(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void PublishEvent(IEvent @event)
        {
            var eventHandlerType = typeof(IHandle<>).MakeGenericType(@event.GetType());
            var listOfEventHandlerTypes = typeof(IEnumerable<>).MakeGenericType(eventHandlerType);
            var handlers = (IEnumerable)_componentContext.Resolve(listOfEventHandlerTypes);
            foreach (var handler in handlers)
            {
                handler.DynamicallyInvokeMethod(OnMissingMethod.Ignore()).Handle(@event);
            }
        }

        public void Send(ICommand command)
        {
            var commandHandlerType = typeof(IHandle<>).MakeGenericType(command.GetType());
            var handler = _componentContext.Resolve(commandHandlerType);
            handler.DynamicallyInvokeMethod(OnMissingMethod.ThrowException()).Handle(command);
        }
    }
}