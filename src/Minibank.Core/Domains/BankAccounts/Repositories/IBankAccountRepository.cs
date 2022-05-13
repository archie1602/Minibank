namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        Task<decimal> GetBalanceAmountByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> IsUserContainsBankAccountsAsync(int userId, CancellationToken cancellationToken);
        Task<bool> IsClosedAsync(int id, CancellationToken cancellationToken);
        Task<bool> IsExistsAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<BankAccount>> GetUserBankAccountsAsync(int userId, CancellationToken cancellationToken);
        Task<BankAccount> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<BankAccount> GetByIdOrDefaultAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellationToken);
        Task CreateAsync(int userId, CurrencyType currencyType, CancellationToken cancellationToken);
        Task CloseAsync(int id, CancellationToken cancellationToken);
        Task TransferAsync(decimal amountToGet, decimal amountToSend, int fromAccountId, int toAccountId, CancellationToken cancellationToken);
    }
}
