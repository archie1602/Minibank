namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        Task<BankAccount> GetAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellationToken);

        Task CreateAsync(int userId, string currencyType, CancellationToken cancellationToken);
        Task CloseAsync(int id, CancellationToken cancellationToken);
        Task<CurrencyInfo> CalculateTransferFeesAsync(decimal amount, int fromAccountId, int toAccountId, CancellationToken cancellationToken);
        Task TransferAsync(decimal amount, int fromAccountId, int toAccountId, CancellationToken cancellationToken);
    }
}
