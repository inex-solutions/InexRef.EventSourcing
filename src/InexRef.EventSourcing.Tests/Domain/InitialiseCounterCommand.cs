using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Tests.Domain
{
    public class InitialiseCounterCommand : Command<string>
    {
        public InitialiseCounterCommand(MessageMetadata messageMetadata, string counterId) : base(messageMetadata, counterId)
        {

        }
    }
}