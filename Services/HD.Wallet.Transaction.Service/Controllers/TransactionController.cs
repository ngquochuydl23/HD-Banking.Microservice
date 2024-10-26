using AutoMapper;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Attributes;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Queries;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Transaction.Service.Dtos.Funds;
using HD.Wallet.Transaction.Service.Dtos.Transfers;
using HD.Wallet.Transaction.Service.Dtos.Withdrawls;
using HD.Wallet.Transaction.Service.ExternalServices;
using HD.Wallet.Transaction.Service.FilterQueries;
using HD.Wallet.Transaction.Service.Infrastructure.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HD.Wallet.Transaction.Service.Controllers
{
    [Authorize]
    [Route("transaction-api/[controller]")]
    public class TransactionController : BaseController
    {
        private readonly AccountExternalService _accountExternalService;
        private readonly IEfRepository<TransactionEntity, string> _transactionRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionController(
          IEfRepository<TransactionEntity, string> transactionRepo,
          IHttpContextAccessor httpContextAccessor,
          IUnitOfWork unitOfWork,
          AccountExternalService accountExternalService,
          IMapper mapper) : base(httpContextAccessor)
        {
            _transactionRepo = transactionRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountExternalService = accountExternalService;
        }


        [HttpGet]
        public IActionResult GetTransactions([FromQuery] TransactionFilterQuery filterQuery)
        {
            var transactions = _transactionRepo
                .GetQueryableNoTracking()
                .WhereIf(!string.IsNullOrEmpty(filterQuery.TransactionStatus), x => x.TransactionStatus.Equals(filterQuery.TransactionStatus))
                .WhereIf(!string.IsNullOrEmpty(filterQuery.TransactionType), x => x.TransactionType.Equals(filterQuery.TransactionType))
                .ToList();

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public IActionResult GetTransactionById(string id)
        {
            var transaction = _transactionRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.Id.Equals(id))
                    ?? throw new AppException("Transaction not found");

            return Ok(transaction);
        }

       

        [ServiceFilter(typeof(PinRequiredAttribute))]
        [HttpPost("withdrawal")]
        public IActionResult Withdrawal([FromBody] RequestWithdrawalDto body)
        {
            return Ok(body);
        }

        [ServiceFilter(typeof(PinRequiredAttribute))]
        [HttpPost("fund")]
        public IActionResult Fund([FromBody] RequestFundDto body)
        {
            return Ok(body);
        }


    }
}
