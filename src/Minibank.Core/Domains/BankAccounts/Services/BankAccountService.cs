using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IBankAccountRepository _bankAccountRepo;
        private readonly IUserRepository _userRepo;
        private readonly ITransferHistoryRepository _transferRepo;

        private readonly ICurrencyConverter _currencyConverter;

        private readonly IValidator<BankAccount> _accountValidator;

        public BankAccountService(IUnitOfWork unitOfWork,
                                  IBankAccountRepository bankAccountRepo, IUserRepository userRepo, ITransferHistoryRepository transferRepo,
                                  ICurrencyConverter currencyConverter,
                                  IValidator<BankAccount> accountValidator) =>
            (_unitOfWork, _bankAccountRepo, _userRepo, _transferRepo, _currencyConverter, _accountValidator) = (unitOfWork, bankAccountRepo, userRepo, transferRepo, currencyConverter, accountValidator);

        private decimal GetTransferFees(decimal amount, int sourceUserId, int destUserId) =>
            sourceUserId == destUserId ? 0 : Math.Round(amount * 2 / 100, 2, MidpointRounding.AwayFromZero);

        public async Task<CurrencyInfo> CalculateTransferFeesAsync(decimal amount, int fromAccountId, int toAccountId, CancellationToken cancellationToken)
        {
            if (amount < 0)
                throw new ValidationException("The amount must be a positive number!");

            //await _accountValidator.ValidateAndThrowAsync(new() { Amount = amount }, cancellationToken);

            //var res = await _accountValidator.ValidateAsync(
            //    new() { Amount = amount },
            //    options => options.IncludeRuleSets("Amount"),
            //    cancellationToken);

            //if (!res.IsValid)
            //    throw new ValidationException("Amount must be a positive number!");

            var sourceBankAccount = await _bankAccountRepo.GetByIdOrDefaultAsync(fromAccountId, cancellationToken);

            if (sourceBankAccount is null)
                throw new ValidationException("There is no source bank account with the specified id!");

            var destBankAccount = await _bankAccountRepo.GetByIdOrDefaultAsync(toAccountId, cancellationToken);

            if (destBankAccount is null)
                throw new ValidationException("There is no destination bank account with the specified id!");

            return new()
            {
                Type = sourceBankAccount.CurrencyType,
                Value = GetTransferFees(amount, sourceBankAccount.UserId, destBankAccount.UserId)
            };
        }

        public async Task CloseAsync(int id, CancellationToken cancellationToken)
        {
            if (await _bankAccountRepo.IsClosedAsync(id, cancellationToken))
                throw new ValidationException("The account is already closed!");

            if (await _bankAccountRepo.GetBalanceAmountByIdAsync(id, cancellationToken) != 0)
                throw new ValidationException("Cannot close a bank account with a non-zero balance!");

            await _bankAccountRepo.CloseAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateAsync(int userId, string currencyType, CancellationToken cancellationToken)
        {
            if (!await _userRepo.IsExistsAsync(userId, cancellationToken))
                throw new ValidationException("The user with the specified id doesn't exist!");

            var validCurrsTypes = new CurrencyType[]
            {
                CurrencyType.RUB,
                CurrencyType.USD,
                CurrencyType.EUR
            };

            if (!Enum.TryParse<CurrencyType>(currencyType, true, out var ct) ||
                !validCurrsTypes.Contains(ct))
                throw new ValidationException("Invalid currency type!");

            await _bankAccountRepo.CreateAsync(userId, ct, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task TransferAsync(decimal amount, int fromAccountId, int toAccountId, CancellationToken cancellationToken)
        {
            if (amount < 0)
                throw new ValidationException("The amount must be a positive number!");

            var senderAccount = await _bankAccountRepo.GetByIdOrDefaultAsync(fromAccountId, cancellationToken);

            if (senderAccount is null)
                throw new ValidationException("There is no sender bank account with the specified id!");

            var recipientAccount = await _bankAccountRepo.GetByIdOrDefaultAsync(toAccountId, cancellationToken);

            if (recipientAccount is null)
                throw new ValidationException("There is no recipient bank account with the specified id!");

            if (senderAccount.IsClosed)
                throw new ValidationException("Cannot transfer money from a closed account!");

            if (recipientAccount.IsClosed)
                throw new ValidationException("Cannot transfer money to a closed account!");

            var diff = senderAccount.Amount - amount;

            if (diff < 0)
                throw new ValidationException("Not enough money in the account!");

            var transferFee = GetTransferFees(amount, senderAccount.UserId, recipientAccount.UserId);

            decimal amountToSend = amount - transferFee;

            var senderCT = senderAccount.CurrencyType;
            var recipientCT = recipientAccount.CurrencyType;

            if (senderCT != recipientCT)
                amountToSend = await _currencyConverter.CurrencyConverterAsync(amountToSend, senderCT, recipientCT, cancellationToken);

            await _bankAccountRepo.TransferAsync(amount, amountToSend, fromAccountId, toAccountId, cancellationToken);

            await _transferRepo.CreateAsync(new TransferHistory()
            {
                Amount = amount,
                CurrencyType = senderCT,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            }, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<BankAccount> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await _bankAccountRepo.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _bankAccountRepo.GetAllAsync(cancellationToken);
        }
    }
}
