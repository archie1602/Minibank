namespace Minibank.Core.Domains.BankAccounts
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public bool IsClosed { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public override bool Equals(object obj) => obj is BankAccount account && (this == account);

        public static bool operator ==(BankAccount a, BankAccount b) =>
            a.Id == b.Id &&
            a.UserId == b.UserId &&
            Math.Abs(a.Amount - b.Amount) < 0.00001M &&
            a.CurrencyType == b.CurrencyType &&
            a.IsClosed == b.IsClosed &&
            a.OpenDate == b.OpenDate &&
            a.CloseDate == b.CloseDate;

        public static bool operator !=(BankAccount a, BankAccount b) => !(a == b);
    }
}
