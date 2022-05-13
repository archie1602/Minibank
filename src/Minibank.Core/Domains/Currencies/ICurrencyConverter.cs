namespace Minibank.Core.Domains.Currencies
{
    public interface ICurrencyConverter
    {
        Task<decimal> CurrencyConverterAsync(decimal amount, CurrencyType fromCurrency, CurrencyType toCurrency, CancellationToken cancellationToken);
    }
}
