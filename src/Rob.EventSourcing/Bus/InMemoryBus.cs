using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Messages;

namespace Rob.EventSourcing.Bus
{
    public class InMemoryBus : IBus
    {
        public readonly ConcurrentDictionary<Type, List<Action<Event>>> _eventHandlers = new ConcurrentDictionary<Type, List<Action<Event>>>();
        public readonly ConcurrentDictionary<Type, Action<Command>> _commandHandlers = new ConcurrentDictionary<Type, Action<Command>>();

        public void Subscribe<T>(Action<T> handler) where T : Event
        {
            // WARNING - this inner list isn't thread safe
            Action<Event> eventHandler = @event => handler((T) @event);
            _eventHandlers.AddOrUpdate(
                key: typeof(T),
                addValue: new List<Action<Event>>(new[] { eventHandler }), 
                updateValueFactory: (id, list) =>
                {
                    var newList = list.Concat(new [] { eventHandler }).ToList();
                    return newList;
                });
        }

        public void PublishEvent(Event @event)
        {
            List<Action<Event>> handlers;

            if (_eventHandlers.TryGetValue(@event.GetType(), out handlers))
            {
                foreach (var handler in handlers)
                {
                    handler(@event);
                }
            }
        }

        public void RegisterHandler<T>(Action<T> handler) where T : Command
        {
            if (!_commandHandlers.TryAdd(typeof(T), t => handler((T)t)))
            {
                throw new HandlerRegistrationException($"Couldn't register handler for type '{typeof(T).Name}', as a registration already exists");
            }
        }

        public void Send<T>(T command) where T : Command
        {
            Action<Command> handler;

            if (!_commandHandlers.TryGetValue(typeof(T), out handler))
            {
                throw new HandlerNotFoundException($"No handler found for command of type '{typeof(T).Name}'");
            }

            handler(command);
        }
    }
}