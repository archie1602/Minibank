namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext appDbContext, IMapper mapper) =>
            (_appDbContext, _mapper) = (appDbContext, mapper);

        public async Task CreateAsync(User user, CancellationToken cancellationToken) =>
            await _appDbContext.Users.AddAsync(_mapper.Map<UserDbModel>(user), cancellationToken);

        public async Task<User> GetByIdOrDefaultAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return entity is null ? null : _mapper.Map<User>(entity);
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdOrDefaultAsync(id, cancellationToken);

            if (entity is null)
                throw new NotFoundException("There is no user with id " + id);

            return entity;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken) =>
            await _appDbContext.Users
            .Select(entity => _mapper.Map<User>(entity))
            .ToListAsync(cancellationToken);

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var userDb = await _appDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (userDb is null)
                throw new NotFoundException("There is no user with id " + id);

            _appDbContext.Users.Remove(userDb);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var userDb = await _appDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);

            if (userDb is null)
                throw new NotFoundException("The user with the specified id doesn't exist");

            userDb.Login = user.Login;
            userDb.Email = user.Email;
        }

        public async Task<bool> IsExistsAsync(int id, CancellationToken cancellationToken) => await _appDbContext.Users.AnyAsync(u => u.Id == id, cancellationToken);
    }
}
