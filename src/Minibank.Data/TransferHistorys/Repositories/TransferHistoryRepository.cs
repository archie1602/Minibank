namespace Minibank.Data.TransferHistorys.Repositories
{
    public class TransferHistoryRepository : ITransferHistoryRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public TransferHistoryRepository(AppDbContext appDbContext, IMapper mapper) =>
            (_appDbContext, _mapper) = (appDbContext, mapper);

        public async Task CreateAsync(TransferHistory info, CancellationToken cancellationToken) =>
            await _appDbContext.TransferHistorys.AddAsync(_mapper.Map<TransferHistoryDbModel>(info), cancellationToken);

        public async Task<IEnumerable<TransferHistory>> GetAllAsync(CancellationToken cancellationToken) =>
            await _appDbContext.TransferHistorys
            .Select(entity => _mapper.Map<TransferHistory>(entity))
            .ToListAsync(cancellationToken);
    }
}