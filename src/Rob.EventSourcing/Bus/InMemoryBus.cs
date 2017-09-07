#region Copyright & License
// Copyright 2017 INEX Solutions Ltd
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Contracts.Bus;
using Rob.EventSourcing.Contracts.Messages;

namespace Rob.EventSourcing.Bus
{
    public class InMemoryBus : IBus
    {
        public readonly ConcurrentDictionary<Type, List<Action<IEvent>>> _eventHandlers = new ConcurrentDictionary<Type, List<Action<IEvent>>>();
        public readonly ConcurrentDictionary<Type, Action<ICommand>> _commandHandlers = new ConcurrentDictionary<Type, Action<ICommand>>();

        public void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            Action<IEvent> eventHandler = @event => handler((T)@event);
            _eventHandlers.AddOrUpdate(
                key: typeof(T),
                addValue: new List<Action<IEvent>>(new[] { eventHandler }), 
                updateValueFactory: (id, list) =>
                {
                    var newList = list.Concat(new [] { eventHandler }).ToList();
                    return newList;
                });
        }

        public void PublishEvent(IEvent @event)
        {
            List<Action<IEvent>> handlers;

            if (_eventHandlers.TryGetValue(@event.GetType(), out handlers))
            {
                foreach (var handler in handlers)
                {
                    handler(@event);
                }
            }
        }

        public void RegisterHandler<T>(Action<T> handler) where T : ICommand
        {
            Action<ICommand> commandHandler = command => handler((T)command);
            if (!_commandHandlers.TryAdd(typeof(T), commandHandler))
            {
                throw new HandlerRegistrationException($"Couldn't register handler for type '{typeof(T).Name}', as a registration already exists");
            }
        }

        public void Send(ICommand command)
        {
            Action<ICommand> handler;

            if (!_commandHandlers.TryGetValue(command.GetType(), out handler))
            {
                throw new HandlerNotFoundException($"No handler found for command of type '{command.GetType().Name}'");
            }

            handler(command);
        }
    }
}