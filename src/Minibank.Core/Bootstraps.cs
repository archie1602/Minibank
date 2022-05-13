namespace Minibank.Core
{
    public static class Bootstraps
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverter, CurrencyConverter>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<ITransferHistoryService, TransferHistoryService>();

            // need some assembly from core project, for example: UserService
            services.AddFluentValidation()
                .AddValidatorsFromAssembly(typeof(UserService).Assembly);

            return services;
        }
    }
}
