namespace Minibank.Core.Domains.Currencies
{
    public class CurrencyInfo
    {
        public CurrencyType Type { get; set; }
        public decimal Value { get; set; }



        public static bool operator ==(CurrencyInfo a, CurrencyInfo b) =>
            a.Type == b.Type &&
            Math.Abs(a.Value - b.Value) < 0.00001M;

        public static bool operator !=(CurrencyInfo a, CurrencyInfo b) => !(a == b);

        public override bool Equals(object obj) => obj is CurrencyInfo ci && this == ci;
    }
}
