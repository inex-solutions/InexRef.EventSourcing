using InexRef.EventSourcing.Common;
using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Contracts.OperationContext
{
    public class OperationContext : IOperationContext, IWriteableOperationContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public OperationContext(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            IsLoading = false;
            SourceMessageMetadata = MessageMetadata.CreateEmpty();
        }

        public MessageMetadata SourceMessageMetadata { get; private set; }

        public bool IsLoading { get; private set; }

        public MessageMetadata CreateNewMessageMetadata()
        {
            return MessageMetadata.Create(SourceMessageMetadata.SourceCorrelationId, _dateTimeProvider.GetUtcNow());
        }

        void IWriteableOperationContext.SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }

        void IWriteableOperationContext.SetMessageMetadata(MessageMetadata messageMetadata)
        {
            SourceMessageMetadata = messageMetadata;
        }
    }
}