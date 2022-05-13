namespace Minibank.Core.Tests.Data.Users
{
    public class ValidateUserWithEmptyLoginData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // incorrect login types
            yield return new object[] { null };
            yield return new object[] { string.Empty };
            yield return new object[] { new string(' ', 1) };
            yield return new object[] { new string(' ', 10) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
