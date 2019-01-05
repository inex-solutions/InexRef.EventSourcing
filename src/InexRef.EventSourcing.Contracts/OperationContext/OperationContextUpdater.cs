using System;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Contracts.OperationContext
{
    public class OperationContextUpdater : IUpdateOperationContext
    {
        private readonly IWriteableOperationContext _context;

        public OperationContextUpdater(IWriteableOperationContext context)
        {
            _context = context;
        }

        public void SetMessageMetadataOnOperationContext(string operationCorrelationId, DateTime operationDateTime)
        {
            _context.SetMessageMetadata(MessageMetadata.Create(operationCorrelationId, operationDateTime));
        }
    }
}