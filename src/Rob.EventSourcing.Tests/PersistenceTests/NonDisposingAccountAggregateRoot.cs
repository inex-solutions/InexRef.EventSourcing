using System;

namespace Rob.EventSourcing.Tests.PersistenceTests
{
    public class NonDisposingAccountAggregateRoot : AccountAggregateRoot
    {
        public NonDisposingAccountAggregateRoot(Guid id) : base(id)
        {

        }

        protected override void Dispose(bool disposing)
        {
            // override the default dispose behaviour (which is to render the aggregate unusable on saving), so that 
            // it can be saved multiple times in order to test concurrency handling
        }
    }
}