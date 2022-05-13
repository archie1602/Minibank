namespace Minibank.Data.Users
{
    [Table("user")]
    public class UserDbModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        internal class Map : IEntityTypeConfiguration<UserDbModel>
        {
            public void Configure(EntityTypeBuilder<UserDbModel> builder)
            {
                builder.Property(it => it.Id);

                builder.Property(it => it.Login);

                builder.Property(it => it.Email);
            }
        }
    }
}