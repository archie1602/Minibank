using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _userRepo;
        private readonly IBankAccountRepository _bankAccountRepo;

        private readonly IValidator<User> _userValidator;
        private readonly IValidator<BankAccount> _accountValidator;

        public UserService(IUnitOfWork unitOfWork,
                           IUserRepository userRepo, IBankAccountRepository bankAccountRepo,
                           IValidator<User> userValidator, IValidator<BankAccount> accountValidator)
            => (_unitOfWork, _userRepo, _bankAccountRepo, _userValidator, _accountValidator) = (unitOfWork, userRepo, bankAccountRepo, userValidator, accountValidator);

        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            // checking if user-model is valid
            await _userValidator.ValidateAndThrowAsync(user, cancellationToken);

            await _userRepo.CreateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<User> GetAsync(int id, CancellationToken cancellationToken) => await _userRepo.GetByIdAsync(id, cancellationToken);

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken) => await _userRepo.GetAllAsync(cancellationToken);

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            // checking if user-model is valid
            await _userValidator.ValidateAndThrowAsync(user, cancellationToken);

            await _userRepo.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (await _bankAccountRepo.IsUserContainsBankAccountsAsync(id, cancellationToken))
                throw new ValidationException("Cannot delete a user with existing bank accounts!");

            await _userRepo.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<BankAccount>> GetUserBankAccountsAsync(int userId, CancellationToken cancellationToken)
        {
            if (!await _userRepo.IsExistsAsync(userId, cancellationToken))
                throw new NotFoundException("The user with the specified id doesn't exist");

            //await _userValidator.ValidateAndThrowAsync(new User() { Id = userId }, cancellationToken);
            return await _bankAccountRepo.GetUserBankAccountsAsync(userId, cancellationToken);
        }
    }
}
