using AutoMapper;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.SharedDtos.Transactions;
using HD.Wallet.Transaction.Service.Dtos.Funds;
using HD.Wallet.Transaction.Service.ExternalServices;
using HD.Wallet.Transaction.Service.Infrastructure.Transactions;
using HD.Wallet.Transaction.Service.Producers.TransactionProducer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HD.Wallet.Transaction.Service.Controllers
{

    [Authorize]
    [Route("transaction-api/[controller]")]
    public class FundController : BaseController
    {

        private readonly AccountExternalService _accountExternalService;
        private readonly BankExternalService _bankExternalService;
        private readonly IEfRepository<TransactionEntity, string> _transactionRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionProducer _transactionProducer;

        public FundController(
          IEfRepository<TransactionEntity, string> transactionRepo,
          IHttpContextAccessor httpContextAccessor,
          IUnitOfWork unitOfWork,
          AccountExternalService accountExternalService,
          BankExternalService bankExternalService,
          ITransactionProducer transactionProducer,
          IMapper mapper) : base(httpContextAccessor)
        {
            _transactionRepo = transactionRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountExternalService = accountExternalService;
            _bankExternalService = bankExternalService;
            _transactionProducer = transactionProducer;
        }


        [HttpPost]
        public async IActionResult Fund([FromBody] RequestFundDto body)
        {
            var sourceAccount = await _accountExternalService.GetAccountById(body.SourceAccountId)
                ?? throw new AppException("Source linking account not found");

            if (!sourceAccount.IsBankLinking)
            {
                throw new AppException("Source account must be linking bank");
            }

            var bankAccount = await _bankExternalService.GetCitizenAccount(sourceAccount.AccountBank.Bin, sourceAccount.AccountBank.BankAccountId)
                ?? throw new AppException("Bank account not found");

            if (bankAccount.Balance < body.Amount)
            {
                throw new AppException("Balance is not enough to fund");
            }


            var transaction = _transactionRepo.Insert(new TransactionEntity()
            {
                Id = Guid
                    .NewGuid()
                    .ToString(),
                Amount = body.Amount,
                SourceAccount = new AccountBankValueObject
                {
                    Bin = bankAccount.Bin,
                    AccountNo = bankAccount.AccountNo,
                    BankName = bankAccount.BankName,
                    OwnerName = bankAccount.OwnerName,
                    ShortName = bankAccount.Bank.ShortName,
                    BankFullName = bankAccount.Bank.Name,
                    LogoUrl = bankAccount.Bank.LogoApp
                },
                //DestAccount = new AccountBankValueObject
                //{
                //    Bin = destBankAccount.Bin,
                //    AccountNo = destBankAccount.AccountNo,
                //    BankName = destBankAccount.BankName,
                //    OwnerName = destBankAccount.OwnerName,
                //    ShortName = destBankAccount.Bank.ShortName,
                //    BankFullName = destBankAccount.Bank.Name,
                //    LogoUrl = destBankAccount.Bank.LogoApp
                //},
                TransactionDate = DateTime.UtcNow,
                TransactionType = TransactionTypeEnum.Transfer,
                TransactionStatus = TransactionStatusEnum.Completed,
                Description = "",
                //TransferContent = body.TransferContent,
                IsBankingTransfer = true,
                UseSourceAsLinkingBank = false,
            });

            var transactionDto = _mapper.Map<TransactionDto>(transaction);

            await _transactionProducer.ProduceTransaction(transactionDto);
            return Ok(transactionDto);
        }

        
    }
}
