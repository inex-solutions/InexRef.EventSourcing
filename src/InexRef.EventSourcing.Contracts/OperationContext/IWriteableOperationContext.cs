using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Contracts.OperationContext
{
    public interface IWriteableOperationContext
    {
        void SetIsLoading(bool isLoading);

        void SetMessageMetadata(MessageMetadata messageMetadata);
    }
}