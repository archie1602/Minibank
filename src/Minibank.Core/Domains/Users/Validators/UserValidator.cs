using System.Text.RegularExpressions;

namespace Minibank.Core.Domains.Users.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator(IUserRepository userRepo)
        {
            // first set the cascade mode
            CascadeMode = CascadeMode.Stop;

            //RuleFor(x => x).MustAsync((u, ct) => userRepo.IsExistsAsync(u.Id, ct))
            //    .WithName("UserId")
            //    .WithMessage(u => $"There is no user with id {u.Id}");

            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("is empty!")
                .Length(3, 20).WithMessage("must be between 3 and 20 characters!");

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("is empty!")
                .Must(e => regex.Match(e).Success).WithMessage("is incorrect!");

            //RuleFor(x => x.Login.Length)
            //    .GreaterThanOrEqualTo(3)
            //    .WithMessage(u => "must contain more than 2 characters!");


            //RuleFor(x => x.Email)
            //    .Must(e => regex.Match(e).Success)
            //    .WithMessage(u => "is incorrect!");
        }
    }
}