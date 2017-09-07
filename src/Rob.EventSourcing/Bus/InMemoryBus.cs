using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Messages;

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