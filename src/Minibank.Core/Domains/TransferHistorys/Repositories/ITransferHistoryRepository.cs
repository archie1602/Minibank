namespace Minibank.Core.Domains.TransferHistorys.Repositories
{
    public interface ITransferHistoryRepository
    {
        Task CreateAsync(TransferHistory info, CancellationToken cancellationToken);
        Task<IEnumerable<TransferHistory>> GetAllAsync(CancellationToken cancellationToken);
    }
}