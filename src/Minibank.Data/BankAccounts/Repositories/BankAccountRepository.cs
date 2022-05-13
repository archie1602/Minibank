namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public BankAccountRepository(AppDbContext appDbContext, IMapper mapper) =>
            (_appDbContext, _mapper) = (appDbContext, mapper);

        public async Task CloseAsync(int id, CancellationToken cancellationToken)
        {
            var account = await _appDbContext.BankAccounts.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

            if (account is null)
                throw new ValidationException("There is no bank account with id: " + id);

            account.IsClosed = true;
            account.CloseDate = DateTime.UtcNow;
        }

        public async Task CreateAsync(int userId, CurrencyType currencyType, CancellationToken cancellationToken)
        {
            var bankAccountDb = new BankAccountDbModel()
            {
                UserId = userId,
                Amount = 1000,
                CurrencyType = currencyType,
                IsClosed = false,
                OpenDate = DateTime.UtcNow
            };

            await _appDbContext.BankAccounts.AddAsync(bankAccountDb, cancellationToken);
        }

        public async Task<BankAccount> GetByIdOrDefaultAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _appDbContext.BankAccounts.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

            return entity is null ? null : _mapper.Map<BankAccount>(entity);
        }

        public async Task<BankAccount> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var account = await GetByIdOrDefaultAsync(id, cancellationToken);

            if (account is null)
                throw new NotFoundException("There is no bank account with id " + id);

            return account;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAsync(CancellationToken cancellationToken) =>
            await _appDbContext.BankAccounts
            .Select(entity => _mapper.Map<BankAccount>(entity))
            .ToListAsync(cancellationToken);

        public async Task<decimal> GetBalanceAmountByIdAsync(int id, CancellationToken cancellationToken) => (await GetByIdAsync(id, cancellationToken)).Amount;

        public async Task<IEnumerable<BankAccount>> GetUserBankAccountsAsync(int userId, CancellationToken cancellationToken)
            => await _appDbContext.BankAccounts
                    .Where(ba => ba.UserId == userId)
                    .Select(ba => _mapper.Map<BankAccount>(ba))
                    .ToListAsync(cancellationToken);

        public async Task<bool> IsClosedAsync(int id, CancellationToken cancellationToken) => (await GetByIdAsync(id, cancellationToken)).IsClosed;

        public async Task<bool> IsExistsAsync(int id, CancellationToken cancellationToken) => await _appDbContext.BankAccounts.AnyAsync(it => it.Id == id, cancellationToken);

        public async Task<bool> IsUserContainsBankAccountsAsync(int userId, CancellationToken cancellationToken) => await _appDbContext.BankAccounts.AnyAsync(it => it.UserId == userId, cancellationToken);

        public async Task TransferAsync(decimal amountToGet, decimal amountToSend, int fromAccountId, int toAccountId, CancellationToken cancellationToken)
        {
            var source = await _appDbContext.BankAccounts.FirstOrDefaultAsync(entity => entity.Id == fromAccountId, cancellationToken);
            var dest = await _appDbContext.BankAccounts.FirstOrDefaultAsync(entity => entity.Id == toAccountId, cancellationToken);

            if (source is null)
                throw new ValidationException("There is no source bank account with the specified id: " + toAccountId);

            if (dest is null)
                throw new ValidationException("There is no dest bank account with the specified id: " + toAccountId);

            source.Amount -= amountToGet;
            dest.Amount += amountToSend;
        }
    }
}
