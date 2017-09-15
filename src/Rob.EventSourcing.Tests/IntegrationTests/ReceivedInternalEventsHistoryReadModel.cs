#region Copyright & License
// The MIT License (MIT)
// 
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

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Rob.EventSourcing.Contracts.Bus;
using Rob.EventSourcing.Contracts.Messages;

namespace Rob.EventSourcing.Tests.IntegrationTests
{
    public class ReceivedInternalEventsHistoryReadModel : IHandle<AmountAddedEvent>, IHandle<BalanceResetEvent>
    {
        private readonly ConcurrentDictionary<string, List<IEvent<string>>> _receivedEvents = new ConcurrentDictionary<string, List<IEvent<string>>>();

        public void Handle(AmountAddedEvent @event)
        {
            ReceiveEvent(@event);
        }

        public void Handle(BalanceResetEvent @event)
        {
            ReceiveEvent(@event);
        }

        private void ReceiveEvent(IEvent<string> @event)
        {
            _receivedEvents.AddOrUpdate(
                key: @event.Id,
                addValue: new List<IEvent<string>>(new[] { @event }),
                updateValueFactory: (id, entry) => entry.Concat(new[] { @event }).ToList());
        }

        public void Clear()
        {
            _receivedEvents.Clear();
        }

        public IList<IEvent<string>> this[string id]
        {
            get
            {
                List<IEvent<string>> events = null;
                _receivedEvents.TryGetValue(id, out events);
                return events;
            }
        }
    }
}