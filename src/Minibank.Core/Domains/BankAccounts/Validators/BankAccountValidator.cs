namespace Minibank.Core.Domains.BankAccounts.Validators
{
    public class BankAccountValidator : AbstractValidator<BankAccount>
    {
        public BankAccountValidator(IBankAccountRepository bankAccountRepo)
        {
            RuleSet("Amount", () =>
            {
                RuleFor(x => x.Amount)
                    .GreaterThan(0)
                    .WithMessage("must be a positive number!");
            });
        }
    }
}