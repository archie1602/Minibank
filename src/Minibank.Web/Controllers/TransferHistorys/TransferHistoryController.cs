namespace Minibank.Web.Controllers.TransferHistorys
{
    [ApiController]
    [Authorize]
    [Route("/transfer-histories")]
    public class TransferHistoryController : ControllerBase
    {
        private readonly ITransferHistoryService _transferService;
        private readonly IMapper _mapper;

        public TransferHistoryController(ITransferHistoryService transferService, IMapper mapper) =>
            (_transferService, _mapper) = (transferService, mapper);

        /// <summary>
        /// Get all transfer history
        /// </summary>
        /// <returns>all transfer history</returns>
        [HttpGet]
        public async Task<IEnumerable<TransferHistoryDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var transferHistory = await _transferService.GetAllAsync(cancellationToken);

            return transferHistory
                .Select(entity => _mapper.Map<TransferHistoryDto>(entity))
                .ToList();
        }
    }
}