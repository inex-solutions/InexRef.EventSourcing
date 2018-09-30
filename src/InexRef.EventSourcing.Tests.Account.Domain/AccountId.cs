using InexRef.EventSourcing.Domain;

namespace InexRef.EventSourcing.Tests.Account.Domain
{
    public class AccountId : ValueObject<AccountId>
    {
        private readonly string _accountId;

        private AccountId(string accountId)
        {
            _accountId = accountId;
        }

        public static AccountId Parse(string accountId)
            => new AccountId(accountId);

        public static AccountId Null { get; } = AccountId.Parse("");

        public override string ToString() => _accountId;

        public static implicit operator string(AccountId accountId) => accountId._accountId;
    }
}