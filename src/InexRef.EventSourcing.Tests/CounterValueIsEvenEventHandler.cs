using System;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Bus;
using InexRef.EventSourcing.Tests.Domain;

namespace InexRef.EventSourcing.Tests
{
    public class CounterValueIsEvenEventHandler : IHandle<CounterValueIsEvenEvent>
    {
        private readonly Func<CounterValueIsEvenEvent, Task> _onEventReceived;

        public CounterValueIsEvenEventHandler(Func<CounterValueIsEvenEvent, Task> onEventReceived)
        {
            _onEventReceived = onEventReceived;
        }

        public async Task Handle(CounterValueIsEvenEvent message)
        {
            await _onEventReceived(message);
        }
    }
}