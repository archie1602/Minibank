namespace Minibank.Core.Tests.Data.Users
{
    public class ValidateUserWithIncorrectEmailData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new string('a', 1) };
            yield return new object[] { new string('a', 10) };
            yield return new object[] { "@" };
            yield return new object[] { "abc@" };
            yield return new object[] { "abc@email" };
            yield return new object[] { "abc@email." };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
