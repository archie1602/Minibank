namespace Minibank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsExistsAsync(int id, CancellationToken cancellationToken);
        Task<User> GetByIdOrDefaultAsync(int id, CancellationToken cancellationToken);
        Task<User> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
        Task CreateAsync(User user, CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
