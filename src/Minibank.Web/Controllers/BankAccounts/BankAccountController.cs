namespace Minibank.Web.Controllers.BankAccounts
{
    [ApiController]
    [Authorize]
    [Route("/accounts")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly IMapper _mapper;

        public BankAccountController(IBankAccountService bankAccountService, IMapper mapper) =>
            (_bankAccountService, _mapper) = (bankAccountService, mapper);

        /// <summary>
        /// Get all bank accounts
        /// </summary>
        /// <returns>bank accounts</returns>
        [HttpGet]
        public async Task<IEnumerable<BankAccountDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var accounts = await _bankAccountService.GetAllAsync(cancellationToken);

            return accounts
                .Select(entity => _mapper.Map<BankAccountDto>(entity))
                .ToList();
        }

        /// <summary>
        /// Get bank account with the specified id
        /// </summary>
        /// <param name="id">bank account id</param>
        /// <returns>bank account</returns>
        [HttpGet("{id:int}")]
        public async Task<BankAccountDto> Get(int id, CancellationToken cancellationToken)
        {
            var entity = await _bankAccountService.GetAsync(id, cancellationToken);

            return _mapper.Map<BankAccountDto>(entity);
        }

        /// <summary>
        /// Creates a bank account for the specified user
        /// </summary>
        /// <param name="accountDto">account dto: user id, bank account currency type</param>
        [HttpPost]
        public async Task CreateAsync(BankAccountCreateDto accountDto, CancellationToken cancellationToken) =>
            await _bankAccountService.CreateAsync(accountDto.UserId, accountDto.CurrencyType, cancellationToken);

        /// <summary>
        /// Closes the specified user's bank account
        /// </summary>
        /// <param name="id">id</param>
        [HttpPost("{id:int}/close")]
        public async Task CloseAsync(int id, CancellationToken cancellationToken) =>
            await _bankAccountService.CloseAsync(id, cancellationToken);

        /// <summary>
        /// Calculates the commission when transferring between two bank accounts
        /// </summary>
        /// <param name="amount">amount of money</param>
        /// <param name="fromAccountId">source bank account id</param>
        /// <param name="toAccountId">destination bank account id</param>
        /// <returns>transfer commission</returns>
        //[HttpGet("{fromAccountId}/{toAccountId}/{amount:decimal}")]
        [HttpGet("commission")]
        public async Task<BankAccountTransferFeesInfoDto> CalculateTransferFeesAsync(int fromAccountId, int toAccountId, decimal amount, CancellationToken cancellationToken)
        {
            var currInfo = await _bankAccountService.CalculateTransferFeesAsync(amount, fromAccountId, toAccountId, cancellationToken);

            return new BankAccountTransferFeesInfoDto()
            {
                CurrencyType = currInfo.Type.ToString(),
                Commission = currInfo.Value
            };
        }

        /// <summary>
        /// Transfers money from one bank account to another
        /// </summary>
        /// <param name="transferInfo">transfer info: amount of money, source bank account id, destination bank account id</param>
        /// <returns></returns>
        [HttpPost("transfer")]
        public async Task TransferAsync(BankAccountTransferInfoDto transferInfo, CancellationToken cancellationToken) =>
            await _bankAccountService.TransferAsync(transferInfo.Amount, transferInfo.FromAccountId, transferInfo.ToAccountId, cancellationToken);
    }
}