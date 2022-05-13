namespace Minibank.Core.Domains.Currencies
{
    public class CurrencyConverter : ICurrencyConverter
    {
        public readonly ICurrencyRateProvider _currencyRateProvider;

        public CurrencyConverter(ICurrencyRateProvider currencyRateProvider) => _currencyRateProvider = currencyRateProvider;

        public async Task<decimal> CurrencyConverterAsync(decimal amount, CurrencyType fromCurrency, CurrencyType toCurrency, CancellationToken cancellationToken)
        {
            var rates = await _currencyRateProvider.GetAllRatesAsync(cancellationToken);

            var fromRateItem = rates[fromCurrency.ToString()];
            var toRateItem = rates[toCurrency.ToString()];

            var fromRate = fromRateItem.Value / fromRateItem.Nominal;
            var toRate = toRateItem.Value / toRateItem.Nominal;

            return amount * fromRate / toRate;
        }
    }
}