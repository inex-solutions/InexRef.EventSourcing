using InexRef.EventSourcing.Contracts.Messages;

namespace InexRef.EventSourcing.Tests.Account.Messages
{
    public class CreateAccountCommand : ICommand<string>
    {
        public MessageMetadata MessageMetadata { get; }

        public string Id { get; }


        public CreateAccountCommand(MessageMetadata messageMetadata, string id)
        {
            MessageMetadata = messageMetadata;
            Id = id;
        }

        public override string ToString()
        {
            return $"CreateAccountCommand: messageMetadata={MessageMetadata}, id={Id}";
        }
    }
}