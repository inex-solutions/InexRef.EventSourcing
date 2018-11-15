using System;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Tests.Domain;

namespace InexRef.EventSourcing.Tests
{
    public class CounterValueDivisibleByTwoEventHandler : IHandle<CounterValueDivisibleByTwoEvent>
    {
        private readonly Action<CounterValueDivisibleByTwoEvent> _onEventReceived;

        public CounterValueDivisibleByTwoEventHandler(Action<CounterValueDivisibleByTwoEvent> onEventReceived)
        {
            _onEventReceived = onEventReceived;
        }

        public void Handle(CounterValueDivisibleByTwoEvent message)
        {
            _onEventReceived(message);
        }
    }
}