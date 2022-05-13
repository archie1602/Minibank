namespace Minibank.Core.Domains.TransferHistorys.Services
{
    public interface ITransferHistoryService
    {
        Task<IEnumerable<TransferHistory>> GetAllAsync(CancellationToken cancellationToken);
    }
}
