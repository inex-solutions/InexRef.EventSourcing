using InexRef.EventSourcing.Domain;

namespace InexRef.EventSourcing.Tests.Account.Domain
{
    public class Balance : ValueObject<Balance>
    {
        private readonly decimal _balanceAmount;

        private Balance(decimal balanceAmount)
        {
            _balanceAmount = balanceAmount;
        }

        public static Balance FromDecimal(decimal balance)
            => new Balance(balance);

        public static Balance Zero { get; } = Balance.FromDecimal(0);

        public Balance AddDecimal(decimal amountToAdd)
            => new Balance(_balanceAmount + amountToAdd);

        public decimal ToDecimal() => _balanceAmount;
    }
}