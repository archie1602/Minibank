namespace Minibank.Core.Domains.TransferHistorys.Services
{
    public class TransferHistoryService : ITransferHistoryService
    {
        private readonly ITransferHistoryRepository _transferRepo;

        public TransferHistoryService(ITransferHistoryRepository transferRepo) => _transferRepo = transferRepo;

        public async Task<IEnumerable<TransferHistory>> GetAllAsync(CancellationToken cancellationToken) => await _transferRepo.GetAllAsync(cancellationToken);
    }
}