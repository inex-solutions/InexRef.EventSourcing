using System;

namespace InexRef.EventSourcing.Contracts.OperationContext
{
    public interface IUpdateOperationContext
    {
        void SetMessageMetadataOnOperationContext(string operationCorrelationId, DateTime operationDateTime);
    }
}