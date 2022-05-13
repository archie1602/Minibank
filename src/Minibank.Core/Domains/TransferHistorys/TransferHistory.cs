namespace Minibank.Core.Domains.TransferHistorys
{
    public class TransferHistory
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
    }
}
