namespace Minibank.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<BankAccountDbModel> BankAccounts { get; set; }
        public DbSet<TransferHistoryDbModel> TransferHistorys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }

    //public class Factory : IDesignTimeDbContextFactory<AppDbContext>
    //{
    //    public AppDbContext CreateDbContext(string[] args)
    //    {
    //        var connStr = "Host=localhost;Port=5432;Database=minibank-demo;Username=postgres;Password=qazwsxedc";

    //        var options = new DbContextOptionsBuilder()
    //            .UseNpgsql(connStr)
    //            .Options;

    //        return new(options);
    //    }
    //}
}