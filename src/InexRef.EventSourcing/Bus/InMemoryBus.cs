using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Contracts.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace InexRef.EventSourcing.Bus
{
    public class InMemoryBus : IBus
    {
        private readonly IServiceProvider _serviceProvider;

        public InMemoryBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishEvent(IEvent @event)
        {
            var eventHandlerType = typeof(IHandle<>).MakeGenericType(@event.GetType());
            var handlers = (IEnumerable)_serviceProvider.GetServices(eventHandlerType);

            List<Task> tasks = new List<Task>();

            foreach (dynamic handler in handlers)
            {
                tasks.Add(handler.Handle((dynamic)@event));
            }

            Task.WaitAll(tasks.ToArray());
            await Task.CompletedTask;
        }

        public async Task Send(ICommand command)
        {
            var commandHandlerType = typeof(IHandle<>).MakeGenericType(command.GetType());

            using (var commandScope = _serviceProvider.CreateScope())
            {
                dynamic handler = commandScope.ServiceProvider.GetRequiredService(commandHandlerType);
                await handler.Handle((dynamic)command);
            }  
        }
    }
}