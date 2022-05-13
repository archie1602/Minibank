namespace Minibank.Core.Domains.Currencies
{
    public interface ICurrencyRateProvider
    {
        Task<Dictionary<string, CurrencyResponseValueItem>> GetAllRatesAsync(CancellationToken cancellationToken);
    }
}
