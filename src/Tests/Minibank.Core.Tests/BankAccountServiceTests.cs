using ValidationExceptionOwn = Minibank.Core.Exceptions.ValidationException;
using ValidationException = FluentValidation.ValidationException;

namespace Minibank.Core.Tests
{
    public class BankAccountServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IBankAccountRepository> _mockBankAccountRepo;
        private readonly Mock<ITransferHistoryRepository> _mockTransferRepo;
        private readonly Mock<ICurrencyRateProvider> _mockCurrencyRateProvider;

        private readonly ICurrencyConverter _currencyConverter;

        //private readonly IValidator<BankAccount> _accountValidator;

        //private readonly IBankAccountService _bankAccountService;

        public BankAccountServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockBankAccountRepo = new Mock<IBankAccountRepository>();
            _mockTransferRepo = new Mock<ITransferHistoryRepository>();

            _mockCurrencyRateProvider = new Mock<ICurrencyRateProvider>();

            //_accountValidator = new BankAccountValidator(_mockBankAccountRepo.Object);

            _currencyConverter = new CurrencyConverter(_mockCurrencyRateProvider.Object);

            //_bankAccountService = new BankAccountService(_mockUnitOfWork.Object,
            //                                             _mockBankAccountRepo.Object,
            //                                             _mockUserRepo.Object,
            //                                             _mockTransferRepo.Object,
            //                                             _currencyConverter,
            //                                             _accountValidator);
        }

        private BankAccountService GetBankAccountService(bool isMockValidator = true) =>
            new BankAccountService(_mockUnitOfWork.Object,
                                   _mockBankAccountRepo.Object,
                                   _mockUserRepo.Object,
                                   _mockTransferRepo.Object,
                                   _currencyConverter,
                                   isMockValidator ? Mock.Of<IValidator<BankAccount>>() : new BankAccountValidator(_mockBankAccountRepo.Object));

        [Fact]
        public async Task GetAccount_SuccessPath_ShouldReturnAccount()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();

            var account = new BankAccount()
            {
                Id = id,
                UserId = 1,
                Amount = 1,
                CurrencyType = CurrencyType.RUB,
                IsClosed = false,
                OpenDate = DateTime.UtcNow,
                CloseDate = null
            };

            _mockBankAccountRepo
                .Setup(r => r.GetByIdAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(account));

            // ACT
            var actualAccount = await bankAccountService.GetAsync(id, CancellationToken.None);

            // ASSERT
            Assert.Equal(account, actualAccount);
        }

        [Fact]
        public async Task GetAccount_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();

            _mockBankAccountRepo
                .Setup(r => r.GetByIdAsync(id, CancellationToken.None));

            // ACT
            await bankAccountService.GetAsync(id, CancellationToken.None);

            // ASSERT
            _mockBankAccountRepo.Verify(repo => repo.GetByIdAsync(id, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetAccounts_SuccessPath_ShouldReturnAccounts()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();

            IEnumerable<BankAccount> accounts = new List<BankAccount>()
            {
                new BankAccount()
                {
                    Id = 1,
                    UserId = 1,
                    Amount = 1,
                    CurrencyType = CurrencyType.RUB,
                    IsClosed = false,
                    OpenDate = DateTime.UtcNow,
                    CloseDate = null
                },

                new BankAccount()
                {
                    Id = 2,
                    UserId = 2,
                    Amount = 1,
                    CurrencyType = CurrencyType.USD,
                    IsClosed = false,
                    OpenDate = DateTime.UtcNow,
                    CloseDate = null
                }
            };

            _mockBankAccountRepo
                .Setup(r => r.GetAllAsync(CancellationToken.None))
                .Returns(Task.FromResult(accounts));

            Func<IEnumerable<BankAccount>, IEnumerable<BankAccount>, bool> pred =
                (l1, l2) => l1.Count() == l2.Count() && Enumerable.All(l1, l2.Contains);

            // ACT
            var actualAccounts = await bankAccountService.GetAllAsync(CancellationToken.None);

            // ASSERT
            Assert.True(pred(accounts, actualAccounts));
        }

        [Fact]
        public async Task GetAccounts_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            _mockBankAccountRepo
                .Setup(r => r.GetAllAsync(CancellationToken.None));

            // ACT
            await bankAccountService.GetAllAsync(CancellationToken.None);

            // ASSERT
            _mockBankAccountRepo.Verify(repo => repo.GetAllAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CloseAccount_ThatAlreadyClosed_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();

            _mockBankAccountRepo
                .Setup(r => r.IsClosedAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(true));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.CloseAsync(id, CancellationToken.None));

            // ASSERT
            Assert.Contains("The account is already closed!", exc.Message);
        }

        [Fact]
        public async Task CloseAccount_WithNonZeroBalance_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();

            _mockBankAccountRepo
                .Setup(r => r.GetBalanceAmountByIdAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(1M));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.CloseAsync(id, CancellationToken.None));

            // ASSERT
            Assert.Contains("Cannot close a bank account with a non-zero balance!", exc.Message);
        }

        [Fact]
        public async Task CloseAccount_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();

            _mockBankAccountRepo
                .Setup(r => r.IsClosedAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(false));

            _mockBankAccountRepo
                .Setup(r => r.GetBalanceAmountByIdAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(0M));

            // ACT
            await bankAccountService.CloseAsync(id, CancellationToken.None);

            // ASSERT
            _mockBankAccountRepo.Verify(r => r.CloseAsync(id, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CloseAccount_SuccessPath_ShouldSaveChangesCalledOnce()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();

            _mockBankAccountRepo
                .Setup(r => r.IsClosedAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(false));

            _mockBankAccountRepo
                .Setup(r => r.GetBalanceAmountByIdAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(0M));

            // ACT
            await bankAccountService.CloseAsync(id, CancellationToken.None);

            // ASSERT
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CreateAccount_ForNonExistentUserId_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();

            _mockUserRepo
                .Setup(r => r.IsExistsAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(false));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.CreateAsync(id, "RUB", CancellationToken.None));

            // ASSERT
            Assert.Contains("The user with the specified id doesn't exist!", exc.Message);
        }

        [Theory]
        [ClassData(typeof(CreateAccountWithInvalidCurrencyTypeData))]
        public async Task CreateAccount_WithInvalidCurrencyType_ShouldThrowException(string currencyType)
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();

            _mockUserRepo
                .Setup(r => r.IsExistsAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(true));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.CreateAsync(id, currencyType, CancellationToken.None));

            // ASSERT
            Assert.Contains("Invalid currency type!", exc.Message);
        }

        [Fact]
        public async Task CreateAccount_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();
            var currencyType = CurrencyType.RUB;

            _mockUserRepo
                .Setup(r => r.IsExistsAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(true));

            // ACT
            await bankAccountService.CreateAsync(id, currencyType.ToString(), CancellationToken.None);

            // ASSERT
            _mockBankAccountRepo.Verify(r => r.CreateAsync(id, currencyType, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CreateAccount_SuccessPath_ShouldSaveChangesCalledOnce()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            int id = It.IsAny<int>();
            var currencyType = CurrencyType.RUB;

            _mockUserRepo
                .Setup(r => r.IsExistsAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(true));

            // ACT
            await bankAccountService.CreateAsync(id, currencyType.ToString(), CancellationToken.None);

            // ASSERT
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CalculateTransferFees_WithNegativeAmount_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = -1M;
            int fromAccountId = It.IsAny<int>();
            int toAccountId = It.IsAny<int>();

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.CalculateTransferFeesAsync(amount, fromAccountId, toAccountId, CancellationToken.None));

            // ASSERT
            Assert.Contains("The amount must be a positive number!", exc.Message);
        }

        [Fact]
        public async Task CalculateTransferFees_WithNonExistentSourceAccount_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 5;
            int toAccountId = 10;

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult<BankAccount>(null));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount()));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.CalculateTransferFeesAsync(amount, fromAccountId, toAccountId, CancellationToken.None));

            // ASSERT
            Assert.Contains("There is no source bank account with the specified id!", exc.Message);
        }

        [Fact]
        public async Task CalculateTransferFees_WithNonExistentDestinationAccount_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 5;
            int toAccountId = 10;

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount()));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult<BankAccount>(null));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.CalculateTransferFeesAsync(amount, fromAccountId, toAccountId, CancellationToken.None));

            // ASSERT
            Assert.Contains("There is no destination bank account with the specified id!", exc.Message);
        }

        [Theory]
        [ClassData(typeof(CalculateTransferFeesBetweenAccountsData))]
        public async Task CalculateTransferFees_BetweenAccounts_ShouldTakeRightCommission(decimal amount, BankAccount fromAccount, BankAccount toAccount, CurrencyInfo currencyInfo)
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(fromAccount));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(toAccount));

            // ACT
            var actualCommission = await bankAccountService.CalculateTransferFeesAsync(amount, fromAccount.Id, toAccount.Id, CancellationToken.None);

            // ASSERT
            Assert.Equal(currencyInfo, actualCommission);
        }

        [Fact]
        public async Task Transfer_WithNegativeAmount_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = -1M;

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.TransferAsync(amount, It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None));

            // ASSERT
            Assert.Contains("The amount must be a positive number!", exc.Message);
        }

        [Fact]
        public async Task Transfer_WithNonExistentSenderAccount_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 1;
            int toAccountId = 2;

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult<BankAccount>(null));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.TransferAsync(amount, fromAccountId, toAccountId, CancellationToken.None));

            // ASSERT
            Assert.Contains("There is no sender bank account with the specified id!", exc.Message);
        }

        [Fact]
        public async Task Transfer_WithNonExistentRecepientAccount_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 1;
            int toAccountId = 2;

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount()));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult<BankAccount>(null));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.TransferAsync(amount, fromAccountId, toAccountId, CancellationToken.None));

            // ASSERT
            Assert.Contains("There is no recipient bank account with the specified id!", exc.Message);
        }

        [Fact]
        public async Task Transfer_WithClosedSenderAccount_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 1;
            int toAccountId = 2;

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount() { IsClosed = true }));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount()));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.TransferAsync(amount, fromAccountId, toAccountId, CancellationToken.None));

            // ASSERT
            Assert.Contains("Cannot transfer money from a closed account!", exc.Message);
        }

        [Fact]
        public async Task Transfer_WithClosedRecepientAccount_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 1;
            int toAccountId = 2;

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount() { IsClosed = false }));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount() { IsClosed = true }));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.TransferAsync(amount, fromAccountId, toAccountId, CancellationToken.None));

            // ASSERT
            Assert.Contains("Cannot transfer money to a closed account!", exc.Message);
        }

        [Fact]
        public async Task Transfer_WithNotEnoughAmount_ShouldThrowException()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 1;
            int toAccountId = 2;

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount() { Amount = 0, IsClosed = false }));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount() { IsClosed = false }));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await bankAccountService.TransferAsync(amount, fromAccountId, toAccountId, CancellationToken.None));

            // ASSERT
            Assert.Contains("Not enough money in the account!", exc.Message);
        }

        [Fact]
        public async Task Transfer_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 1;
            int toAccountId = 2;

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount() { Amount = amount, IsClosed = false }));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new BankAccount() { IsClosed = false }));

            // ACT
            await bankAccountService.TransferAsync(amount, fromAccountId, toAccountId, CancellationToken.None);

            // ASSERT
            _mockBankAccountRepo.Verify(r =>
                r.TransferAsync(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Transfer_SuccessPath_CreateTransferHistoryShouldCalledOnce()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 1;
            int toAccountId = 2;

            var fromAccount = new BankAccount()
            {
                Id = 1,
                IsClosed = false,
                Amount = amount,
            };

            var toAccount = new BankAccount()
            {
                Id = 2,
                IsClosed = false,
            };

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(fromAccount));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(toAccount));

            // ACT
            await bankAccountService.TransferAsync(amount, fromAccountId, toAccountId, CancellationToken.None);

            // ASSERT
            _mockTransferRepo.Verify(r =>
                r.CreateAsync(It.IsAny<TransferHistory>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Transfer_SuccessPath_ShouldSaveChangesCalledOnce()
        {
            // ARRANGE
            var bankAccountService = GetBankAccountService();

            var amount = 1M;

            int fromAccountId = 1;
            int toAccountId = 2;

            var fromAccount = new BankAccount()
            {
                Id = 1,
                IsClosed = false,
                Amount = amount,
            };

            var toAccount = new BankAccount()
            {
                Id = 2,
                IsClosed = false,
            };

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(fromAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(fromAccount));

            _mockBankAccountRepo
                .Setup(r => r.GetByIdOrDefaultAsync(toAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(toAccount));

            // ACT
            await bankAccountService.TransferAsync(amount, fromAccountId, toAccountId, CancellationToken.None);

            // ASSERT
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}