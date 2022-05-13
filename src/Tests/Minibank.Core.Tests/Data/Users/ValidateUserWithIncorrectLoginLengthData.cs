namespace Minibank.Core.Tests.Data.Users
{
    public class ValidateUserWithIncorrectLoginLengthData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            int min = 3;
            int max = 20;

            // incorrect login length types
            yield return new object[] { new string('a', 1) };
            yield return new object[] { new string('a', min - 1) };
            yield return new object[] { new string('a', max + 1) };
            yield return new object[] { new string('a', max + 2) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
