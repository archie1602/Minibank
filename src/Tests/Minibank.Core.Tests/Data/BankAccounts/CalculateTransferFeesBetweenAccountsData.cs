using Minibank.Core.Domains.Currencies;

namespace Minibank.Core.Tests.Data.BankAccounts
{
    public class CalculateTransferFeesBetweenAccountsData : IEnumerable<object[]>
    {
        private decimal GetTransferFees(decimal amount, int sourceUserId, int destUserId) =>
            sourceUserId == destUserId ? 0 : Math.Round(amount * 2 / 100, 2, MidpointRounding.AwayFromZero);

        public IEnumerator<object[]> GetEnumerator()
        {
            // between accounts of different users
            yield return new object[]
            {
                1M,
                new BankAccount() { Id = 1, UserId = 1, CurrencyType = CurrencyType.EUR },
                new BankAccount() { Id = 2, UserId = 2, CurrencyType = CurrencyType.RUB },
                new CurrencyInfo()
                {
                    Type = CurrencyType.EUR,
                    Value = GetTransferFees(1M, 1, 2)
                }
            };

            // between accounts of the same user
            yield return new object[]
            {
                1M,
                new BankAccount() { Id = 1, UserId = 1, CurrencyType = CurrencyType.USD },
                new BankAccount() { Id = 2, UserId = 1, CurrencyType = CurrencyType.RUB },
                new CurrencyInfo()
                {
                    Type = CurrencyType.USD,
                    Value = GetTransferFees(1M, 1, 1)
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
