namespace Minibank.Data
{
    public static class Bootstraps
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<ICurrencyRateProvider, CurrencyRateProvider>(options =>
                options.BaseAddress = new Uri(config["CurrencyRateProviderUrl"]));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ITransferHistoryRepository, TransferHistoryRepository>();

            services.AddAutoMapper(typeof(MinibankDataMapper));

            services.AddScoped<IUnitOfWork, EfUnityOfWork>();

            services.AddDbContext<AppDbContext>(
                options =>
                    options
                        .UseNpgsql(config.GetConnectionString("DefaultConnection"))
                        .UseSnakeCaseNamingConvention()
                    );

            return services;
        }
    }
}
