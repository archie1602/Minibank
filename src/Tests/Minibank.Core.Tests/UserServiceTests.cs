using ValidationExceptionOwn = Minibank.Core.Exceptions.ValidationException;
using ValidationException = FluentValidation.ValidationException;

namespace Minibank.Core.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IBankAccountRepository> _mockBankAccountRepo;

        //private readonly Mock<IValidator<User>> _mockUserValidator;
        //private readonly Mock<IValidator<BankAccount>> _mockAccountValidator;

        //private readonly IUserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockBankAccountRepo = new Mock<IBankAccountRepository>();

            //_userValidator = new UserValidator(_mockUserRepo.Object);
            //_accountValidator = new BankAccountValidator(_mockBankAccountRepo.Object);

            //_userService = new UserService(_mockUnitOfWork.Object,
            //                               _mockUserRepo.Object,
            //                               _mockBankAccountRepo.Object,
            //                               _userValidator,
            //                               _accountValidator);
        }

        private UserService GetUserService(bool isMockValidators = true) =>
            new UserService(_mockUnitOfWork.Object,
                            _mockUserRepo.Object,
                            _mockBankAccountRepo.Object,
                            isMockValidators ? Mock.Of<IValidator<User>>() : new UserValidator(_mockUserRepo.Object),
                            isMockValidators ? Mock.Of<IValidator<BankAccount>>() : new BankAccountValidator(_mockBankAccountRepo.Object));


        [Fact]
        public async Task GetUserAccounts_ByNonExistentUserId_ShouldThrowException()
        {
            // ARRANGE
            var userService = GetUserService();

            _mockUserRepo
                .Setup(r => r.IsExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(false));

            // ACT
            var exc = await Assert.ThrowsAsync<NotFoundException>(
                async () => await userService.GetUserBankAccountsAsync(It.IsAny<int>(), CancellationToken.None));

            // ASSERT
            Assert.Contains("The user with the specified id doesn't exist", exc.Message);
        }

        [Fact]
        public async Task GetUserAccounts_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var userService = GetUserService();

            _mockUserRepo
                .Setup(r => r.IsExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(true));

            // ACT
            await userService.GetUserBankAccountsAsync(It.IsAny<int>(), CancellationToken.None);

            // ASSERT
            _mockBankAccountRepo.Verify(ba => ba.GetUserBankAccountsAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetUser_SuccessPath_ShouldReturnUser()
        {
            // ARRANGE
            var userService = GetUserService();

            int id = It.IsAny<int>();

            var user = new User()
            {
                Id = id,
                Login = "login",
                Email = "email@email.com"
            };

            _mockUserRepo
                .Setup(r => r.GetByIdAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(user));

            // ACT
            var actualUser = await userService.GetAsync(id, CancellationToken.None);

            // ASSERT
            Assert.Equal(user, actualUser);
        }

        [Fact]
        public async Task GetUser_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var userService = GetUserService();

            int id = It.IsAny<int>();

            _mockUserRepo
                .Setup(r => r.GetByIdAsync(id, CancellationToken.None));

            // ACT
            await userService.GetAsync(id, CancellationToken.None);

            // ASSERT
            _mockUserRepo.Verify(repo => repo.GetByIdAsync(id, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetUsers_SuccessPath_ShouldReturnUsers()
        {
            // ARRANGE
            var userService = GetUserService();

            IEnumerable<User> users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Login = "login1",
                    Email = "email1@email.com"
                },

                new User()
                {
                    Id = 2,
                    Login = "login2",
                    Email = "email2@email.com"
                }
            };

            _mockUserRepo
                .Setup(r => r.GetAllAsync(CancellationToken.None))
                .Returns(Task.FromResult(users));

            Func<IEnumerable<User>, IEnumerable<User>, bool> pred =
                (l1, l2) => l1.Count() == l2.Count() && Enumerable.All(l1, l2.Contains);

            // ACT
            var actualUsers = await userService.GetAllAsync(CancellationToken.None);

            // ASSERT
            Assert.True(pred(users, actualUsers));
        }

        [Fact]
        public async Task GetUsers_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var userService = GetUserService();

            _mockUserRepo
                .Setup(r => r.GetAllAsync(CancellationToken.None));

            // ACT
            await userService.GetAllAsync(CancellationToken.None);

            // ASSERT
            _mockUserRepo.Verify(repo => repo.GetAllAsync(CancellationToken.None), Times.Once);
        }

        [Theory]
        [ClassData(typeof(ValidateUserWithEmptyLoginData))]
        public async Task CreateUser_WithEmptyLogin_ShouldThrowException(string login)
        {
            // ARRANGE
            var userService = GetUserService(false);

            var user = new User()
            {
                Login = login,
                Email = "email@email.com"
            };

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationException>(
                async () => await userService.CreateAsync(user, CancellationToken.None));

            // ASSERT
            Assert.Contains("Login: is empty!", exc.Message);
        }

        [Theory]
        [ClassData(typeof(ValidateUserWithIncorrectLoginLengthData))]
        public async Task CreateUser_WithIncorrectLoginLength_ShouldThrowException(string login)
        {
            // ARRANGE
            var userService = GetUserService(false);

            var user = new User()
            {
                Login = login,
                Email = "email@email.com"
            };

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationException>(
                async () => await userService.CreateAsync(user, CancellationToken.None));

            // ASSERT
            Assert.Contains("Login: must be between 3 and 20 characters!", exc.Message);
        }

        [Theory]
        [ClassData(typeof(ValidateUserWithEmptyEmailData))]
        public async Task CreateUser_WithEmptyEmail_ShouldThrowException(string email)
        {
            // ARRANGE
            var userService = GetUserService(false);

            var user = new User()
            {
                Login = "login",
                Email = email
            };

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationException>(
                async () => await userService.CreateAsync(user, CancellationToken.None));

            // ASSERT
            Assert.Contains("Email: is empty!", exc.Message);
        }

        [Theory]
        [ClassData(typeof(ValidateUserWithIncorrectEmailData))]
        public async Task CreateUser_WithIncorrectEmail_ShouldThrowException(string email)
        {
            // ARRANGE
            var userService = GetUserService(false);

            var user = new User()
            {
                Login = "login",
                Email = email
            };

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationException>(
                async () => await userService.CreateAsync(user, CancellationToken.None));

            // ASSERT
            Assert.Contains("Email: is incorrect!", exc.Message);
        }

        [Fact]
        public async Task CreateUser_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var userService = GetUserService();

            // ACT
            await userService.CreateAsync(It.IsAny<User>(), CancellationToken.None);

            // ASSERT
            _mockUserRepo.Verify(r => r.CreateAsync(It.IsAny<User>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CreateUser_SuccessPath_ShouldSaveChangesCalledOnce()
        {
            // ARRANGE
            var userService = GetUserService();

            // ACT
            await userService.CreateAsync(It.IsAny<User>(), CancellationToken.None);

            // ASSERT
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Theory]
        [ClassData(typeof(ValidateUserWithEmptyLoginData))]
        public async Task UpdateUser_WithEmptyLogin_ShouldThrowException(string login)
        {
            // ARRANGE
            var userService = GetUserService(false);

            var user = new User()
            {
                Login = login,
                Email = "email@email.com"
            };

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationException>(
                async () => await userService.UpdateAsync(user, CancellationToken.None));

            // ASSERT
            Assert.Contains("Login: is empty!", exc.Message);
        }

        [Theory]
        [ClassData(typeof(ValidateUserWithIncorrectLoginLengthData))]
        public async Task UpdateUser_WithIncorrectLoginLength_ShouldThrowException(string login)
        {
            // ARRANGE
            var userService = GetUserService(false);

            var user = new User()
            {
                Login = login,
                Email = "email@email.com"
            };

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationException>(
                async () => await userService.UpdateAsync(user, CancellationToken.None));

            // ASSERT
            Assert.Contains("Login: must be between 3 and 20 characters!", exc.Message);
        }

        [Theory]
        [ClassData(typeof(ValidateUserWithEmptyEmailData))]
        public async Task UpdateUser_WithEmptyEmail_ShouldThrowException(string email)
        {
            // ARRANGE
            var userService = GetUserService(false);

            var user = new User()
            {
                Login = "login",
                Email = email
            };

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationException>(
                async () => await userService.UpdateAsync(user, CancellationToken.None));

            // ASSERT
            Assert.Contains("Email: is empty!", exc.Message);
        }

        [Theory]
        [ClassData(typeof(ValidateUserWithIncorrectEmailData))]
        public async Task UpdateUser_WithIncorrectEmail_ShouldThrowException(string email)
        {
            // ARRANGE
            var userService = GetUserService(false);

            var user = new User()
            {
                Login = "login",
                Email = email
            };

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationException>(
                async () => await userService.UpdateAsync(user, CancellationToken.None));

            // ASSERT
            Assert.Contains("Email: is incorrect!", exc.Message);
        }

        [Fact]
        public async Task UpdateUser_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var userService = GetUserService();

            int id = It.IsAny<int>();

            var newUser = new User()
            {
                Id = id,
                Login = "login",
                Email = "email@email.com"
            };

            _mockUserRepo
                .Setup(r => r.UpdateAsync(newUser, CancellationToken.None))
                .Returns(Task.CompletedTask);

            // ACT
            await userService.UpdateAsync(newUser, CancellationToken.None);

            // ASSERT
            _mockUserRepo.Verify(repo => repo.UpdateAsync(newUser, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_SuccessPath_ShouldSaveChangesCalledOnce()
        {
            // ARRANGE
            var userService = GetUserService();

            // ACT
            await userService.UpdateAsync(It.IsAny<User>(), CancellationToken.None);

            // ASSERT
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        //[Fact]
        //public async Task CreateUser_SuccessPath_ShouldValidateAndThrowCalledOnce()
        //{
        //    // ARRANGE
        //    var user = It.IsAny<User>();

        //    var userService = GetUserService();

        //    // ACT
        //    await userService.CreateAsync(user, CancellationToken.None);

        //    // ASSERT
        //    _mockUserValidator.Verify(v => v.ValidateAndThrowAsync(user, CancellationToken.None), Times.Once);
        //}

        [Fact]
        public async Task DeleteUser_WithExistingBankAccount_ShouldThrowException()
        {
            // ARRANGE
            var userService = GetUserService(false);

            int id = It.IsAny<int>();

            _mockBankAccountRepo
                .Setup(r => r.IsUserContainsBankAccountsAsync(id, CancellationToken.None))
                .Returns(Task.FromResult(true));

            // ACT
            var exc = await Assert.ThrowsAsync<ValidationExceptionOwn>(
                async () => await userService.DeleteAsync(id, CancellationToken.None));

            // ASSERT
            Assert.Contains("Cannot delete a user with existing bank accounts!", exc.Message);
        }

        [Fact]
        public async Task DeleteUser_SuccessPath_ShouldCalledOnce()
        {
            // ARRANGE
            var userService = GetUserService();

            int id = It.IsAny<int>();

            _mockUserRepo
                .Setup(r => r.DeleteAsync(id, CancellationToken.None))
                .Returns(Task.CompletedTask);

            // ACT
            await userService.DeleteAsync(id, CancellationToken.None);

            // ASSERT
            _mockUserRepo.Verify(repo => repo.DeleteAsync(id, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_SuccessPath_ShouldSaveChangesCalledOnce()
        {
            // ARRANGE
            var userService = GetUserService();

            // ACT
            await userService.DeleteAsync(It.IsAny<int>(), CancellationToken.None);

            // ASSERT
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}