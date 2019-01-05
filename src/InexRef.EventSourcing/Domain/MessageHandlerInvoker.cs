using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Domain
{
    public class MessageHandlerInvoker
    {
        private readonly Dictionary<Type, List<IInvokableMessageHandlerAction>> _messageHandlers 
            = new Dictionary<Type, List<IInvokableMessageHandlerAction>>();

        public void RegisterMessageHandler<TMessage>(Func<TMessage, Task> messageHandler)
            where TMessage : IMessage
        {
            
            if (!_messageHandlers.TryGetValue(typeof(TMessage), out List<IInvokableMessageHandlerAction> handlersForMessageType))
            {
                handlersForMessageType = new List<IInvokableMessageHandlerAction>();
                _messageHandlers.Add(typeof(TMessage), handlersForMessageType);
            }

            handlersForMessageType.Add(new InvokableMessageHandlerAction<TMessage>(messageHandler));
        }

        public async Task Invoke(IMessage message)
        {
            if (_messageHandlers.TryGetValue(message.GetType(), out List <IInvokableMessageHandlerAction> handlersForMessageType))
            {
                foreach (var handlerInvoker in handlersForMessageType)
                {
                    await handlerInvoker.Invoke(message);
                }
            }
        }

        private interface IInvokableMessageHandlerAction
        {
            Task Invoke(IMessage message);
        }

        private class InvokableMessageHandlerAction<TMessage> : IInvokableMessageHandlerAction
        {
            private readonly Func<TMessage, Task> _handler;

            public InvokableMessageHandlerAction(Func<TMessage, Task> handler)
            {
                _handler = handler;
            }

            public async Task Invoke(IMessage message)
            {
                await _handler((TMessage)message);
            }
        }
    }
}