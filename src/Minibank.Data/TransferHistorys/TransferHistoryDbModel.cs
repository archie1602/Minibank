namespace Minibank.Data.TransferHistorys
{
    [Table("transfer_history")]
    public class TransferHistoryDbModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }

        internal class Map : IEntityTypeConfiguration<TransferHistoryDbModel>
        {
            public void Configure(EntityTypeBuilder<TransferHistoryDbModel> builder)
            {
                builder.Property(it => it.Id);

                builder.Property(it => it.Amount);

                builder.Property(it => it.CurrencyType);

                builder.Property(it => it.FromAccountId);

                builder.Property(it => it.ToAccountId);
            }
        }
    }
}
