using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Contracts.OperationContext
{
    public interface IOperationContext
    {
        MessageMetadata SourceMessageMetadata { get; }

        MessageMetadata CreateNewMessageMetadata();

        bool IsLoading { get; }
    }
}