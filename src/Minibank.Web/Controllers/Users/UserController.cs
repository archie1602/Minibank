namespace Minibank.Web.Controllers.Users
{
    [ApiController]
    [Authorize]
    [Route("/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper) =>
            (_userService, _mapper) = (userService, mapper);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>users</returns>
        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllAsync(cancellationToken);

            return users
                .Select(entity => _mapper.Map<UserDto>(entity))
                .ToList();
        }

        /// <summary>
        /// Get all bank accounts of the specified user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>all user bank accounts</returns>
        [HttpGet("{userId:int}/accounts")]
        public async Task<IEnumerable<BankAccountDto>> GetAllUserAccountsAsync(int userId, CancellationToken cancellationToken)
        {
            var accounts = await _userService.GetUserBankAccountsAsync(userId, cancellationToken);

            return accounts
                .Select(entity => _mapper.Map<BankAccountDto>(entity))
                .ToList();
        }

        /// <summary>
        /// Get user with the specified id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user</returns>
        [HttpGet("{id:int}")]
        public async Task<UserDto> GetAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetAsync(id, cancellationToken);

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="userDto">user info</param>
        [HttpPost]
        public async Task CreateAsync(UserCreateDto userDto, CancellationToken cancellationToken) =>
            await _userService.CreateAsync(_mapper.Map<User>(userDto), cancellationToken);

        /// <summary>
        /// Updates the information of the specified user
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="userDto">user info</param>
        [HttpPut("{id:int}")]
        public async Task UpdateAsync(int id, UserUpdateDto userDto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<User>(userDto);
            entity.Id = id;

            await _userService.UpdateAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Deletes a user with the specified id
        /// </summary>
        /// <param name="id">user id</param>
        [HttpDelete("{id:int}")]
        public async Task DeleteAsync(int id, CancellationToken cancellationToken) =>
            await _userService.DeleteAsync(id, cancellationToken);
    }
}