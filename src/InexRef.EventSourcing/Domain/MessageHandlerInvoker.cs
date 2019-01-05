#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
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