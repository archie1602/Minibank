namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task<User> GetAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<BankAccount>> GetUserBankAccountsAsync(int userId, CancellationToken cancellationToken);
        Task CreateAsync(User user, CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
