namespace Minibank.Core.Domains.Currencies
{
    public class CurrencyResponseValueItem
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public decimal Value { get; set; }
    }
}
