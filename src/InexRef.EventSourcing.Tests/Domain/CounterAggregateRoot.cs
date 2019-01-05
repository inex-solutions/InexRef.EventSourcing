using System;
using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.OperationContext;
using InexRef.EventSourcing.Domain;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class CounterAggregateRoot : AggregateRoot<Guid>
    {
        public override string Name => "Counter";

        public int CurrentValue { get; private set; }

        public CounterAggregateRoot(IOperationContext operationContext) : base(operationContext)
        {
            RegisterEventHandler<CounterInitialisedEvent>(HandleEvent);
            RegisterEventHandler<CounterIncrementedEvent>(HandleEvent);
        }

        public async Task Initialise(Guid id)
        {
            await Apply(new CounterInitialisedEvent(OperationContext.CreateNewMessageMetadata(), id));
        }

        public async Task Increment()
        {
            await Apply(new CounterIncrementedEvent(OperationContext.CreateNewMessageMetadata(), Id));
        }

        private async Task HandleEvent(CounterInitialisedEvent @event)
        {
            Id = @event.Id;
            await Task.CompletedTask;
        }

        private async Task HandleEvent(CounterIncrementedEvent @event)
        {
            CurrentValue++;
            if (CurrentValue % 2 == 0)
            {
                await Apply(new CounterValueIsEvenEvent(OperationContext.CreateNewMessageMetadata(), Id, CurrentValue));
            }
        }
    }
}