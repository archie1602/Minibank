namespace Minibank.Data
{
    public class EfUnityOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public EfUnityOfWork(AppDbContext appDbContext) => _appDbContext = appDbContext;

        public int SaveChanges() => _appDbContext.SaveChanges();
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
