namespace Minibank.Data.Currencies
{
    public class CurrencyRateProvider : ICurrencyRateProvider
    {
        private readonly HttpClient _httpClient;

        public CurrencyRateProvider(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<Dictionary<string, CurrencyResponseValueItem>> GetAllRatesAsync(CancellationToken cancellationToken)
        {
            var resp = await _httpClient.GetFromJsonAsync<CurrencyRateResponse>("daily_json.js", cancellationToken);

            // add the ruble artificially, cause the API doesn't contain it
            var RUB = CurrencyType.RUB.ToString();

            resp.Valute.Add(RUB,
                            new()
                            {
                                CharCode = RUB,
                                Nominal = 1,
                                Value = 1
                            });

            return resp.Valute;
        }
    }
}
