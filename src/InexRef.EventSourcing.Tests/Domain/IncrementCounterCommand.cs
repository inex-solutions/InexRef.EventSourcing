using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class IncrementCounterCommand : Command<string>
    {
        public IncrementCounterCommand(MessageMetadata messageMetadata, string counterId) : base(messageMetadata, counterId)
        {
        }
    }
}