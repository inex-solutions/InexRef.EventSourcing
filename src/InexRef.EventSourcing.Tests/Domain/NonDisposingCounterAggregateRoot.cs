using InexRef.EventSourcing.Contracts.OperationContext;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class NonDisposingCounterAggregateRoot : CounterAggregateRoot
    {
        public NonDisposingCounterAggregateRoot(IOperationContext operationContext) : base(operationContext)
        {
        }

        protected override void Dispose(bool disposing)
        {
            // override the default dispose behaviour (which is to render the aggregate unusable on saving), so that 
            // it can be saved multiple times in order to test concurrency handling
        }
    }
}