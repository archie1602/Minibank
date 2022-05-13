namespace Minibank.Core.Domains.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        public static bool operator ==(User a, User b) =>
            a.Id == b.Id &&
            a.Login == b.Login &&
            a.Email == b.Email;

        public static bool operator !=(User a, User b) => !(a == b);

        public override bool Equals(object obj) => obj is User user && (this == user);
    }
}
