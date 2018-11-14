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
// NONINFRINGEMENT. IN NO Message SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Domain
{
    public class MessageHandlerInvoker
    {
        private readonly Dictionary<Type, IInvokableMessageHandlerAction> _messageHandlers 
            = new Dictionary<Type, IInvokableMessageHandlerAction>();

        public void RegisterMessageHandler<TMessage>(Action<TMessage> messageHandler)
            where TMessage : IMessage
        {
            _messageHandlers.Add(typeof(TMessage), new InvokableMessageHandlerAction<TMessage>(messageHandler));
        }

        public void Invoke(IMessage message)
        {
            if (_messageHandlers.TryGetValue(message.GetType(), out IInvokableMessageHandlerAction handlerInvoker))
            {
                handlerInvoker.Invoke(message);
            }
        }

        private interface IInvokableMessageHandlerAction
        {
            void Invoke(IMessage message);
        }

        private class InvokableMessageHandlerAction<TMessage> : IInvokableMessageHandlerAction
        {
            private readonly Action<TMessage> _handler;

            public InvokableMessageHandlerAction(Action<TMessage> handler)
            {
                _handler = handler;
            }

            public void Invoke(IMessage message)
            {
                _handler((TMessage)message);
            }
        }
    }
}