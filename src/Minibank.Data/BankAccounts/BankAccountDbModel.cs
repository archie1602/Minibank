namespace Minibank.Data.BankAccounts
{
    [Table("bank_account")]
    public class BankAccountDbModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public bool IsClosed { get; set; } = true;
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

        internal class Map : IEntityTypeConfiguration<BankAccountDbModel>
        {
            public void Configure(EntityTypeBuilder<BankAccountDbModel> builder)
            {
                builder.Property(it => it.Id);

                builder.Property(it => it.UserId);

                builder.Property(it => it.Amount);

                builder.Property(it => it.CurrencyType);

                builder.Property(it => it.IsClosed);

                builder.Property(it => it.OpenDate);

                builder.Property(it => it.CloseDate);
            }
        }
    }
}
