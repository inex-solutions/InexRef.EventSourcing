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

using System;
using System.Collections.Generic;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Domain
{
    public class EventHandlerInvoker<TEventId> where TEventId : IEquatable<TEventId>, IComparable<TEventId>
    {
        private readonly Dictionary<Type, IInvokableEventHandlerAction<TEventId>> _eventHandlers 
            = new Dictionary<Type, IInvokableEventHandlerAction<TEventId>>();

        public void RegisterEventHandler<TEvent>(Action<TEvent> eventHandler)
            where TEvent : IEvent<TEventId>
        {
            _eventHandlers.Add(typeof(TEvent), new InvokableEventHandlerAction<TEvent, TEventId>(eventHandler));
        }

        public void Invoke(IEvent<TEventId> @event)
        {
            if (_eventHandlers.TryGetValue(@event.GetType(), out IInvokableEventHandlerAction<TEventId> handlerInvoker))
            {
                handlerInvoker.Invoke(@event);
            }
        }

        private interface IInvokableEventHandlerAction<in TId> where TId : IEquatable<TId>, IComparable<TId>
        {
            void Invoke(IEvent<TId> @event);
        }

        private class InvokableEventHandlerAction<TEvent, TId> : IInvokableEventHandlerAction<TId>
            where TId : IEquatable<TId>, IComparable<TId>
        {
            private readonly Action<TEvent> _handler;

            public InvokableEventHandlerAction(Action<TEvent> handler)
            {
                _handler = handler;
            }

            public void Invoke(IEvent<TId> @event)
            {
                _handler((TEvent)@event);
            }
        }
    }
}