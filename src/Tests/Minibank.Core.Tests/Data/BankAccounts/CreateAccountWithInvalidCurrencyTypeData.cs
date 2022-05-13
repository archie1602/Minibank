namespace Minibank.Core.Tests.Data.BankAccounts
{
    public class CreateAccountWithInvalidCurrencyTypeData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null };
            yield return new object[] { string.Empty };
            yield return new object[] { new string(' ', 1) };
            yield return new object[] { new string(' ', 10) };
            yield return new object[] { "ABC" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
