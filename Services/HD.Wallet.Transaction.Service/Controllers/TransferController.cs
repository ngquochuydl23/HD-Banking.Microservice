using AutoMapper;
using Confluent.Kafka;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Attributes;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.SharedDtos.Transactions;
using HD.Wallet.Transaction.Service.Dtos.Transfers;
using HD.Wallet.Transaction.Service.ExternalServices;
using HD.Wallet.Transaction.Service.Infrastructure.Transactions;
using HD.Wallet.Transaction.Service.Producers.TransactionProducer;
using Mailjet.Client.Resources.SMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;


namespace HD.Wallet.Transaction.Service.Controllers
{
    [Authorize]
    [Route("transaction-api/[controller]")]
    public class TransferController : BaseController
    {

        private readonly AccountExternalService _accountExternalService;
        private readonly BankExternalService _bankExternalService;
        private readonly IEfRepository<TransactionEntity, string> _transactionRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionProducer _transactionProducer;

        public TransferController(
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

        [ServiceFilter(typeof(PinRequiredAttribute))]
        [HttpPost("BankTransfer")]
        public async Task<IActionResult> BankTransfer(
           [FromHeader(Name = "X-EncryptedPin")] string pin,
           [FromBody] RequestBankTransferDto body)
        {


            var destBankAccount = await _bankExternalService.GetCitizenAccount(body.DestBin, body.DestBankAccountNo)
                ?? throw new AppException("Destination bank account not found");


            if (!body.UseLinkingBank)
            {
                var walletAccount = await _accountExternalService.GetAccountById(body.SourceAccountId)
                    ?? throw new AppException("Wallet account not found");

                if (walletAccount.WalletBalance < body.TransferAmount)
                {
                    throw new AppException("Balance is not enough to transfer");
                }

                var transaction = _transactionRepo.Insert(new TransactionEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = body.TransferAmount,
                    SourceAccount = new AccountBankValueObject
                    {
                        Bin = walletAccount.AccountBank.Bin,
                        AccountNo = walletAccount.AccountBank.BankAccountId,
                        BankName = walletAccount.AccountBank.BankName,
                        OwnerName = walletAccount.AccountBank.BankOwnerName,
                        BankFullName = walletAccount.AccountBank.BankFullName,
                        LogoUrl = walletAccount.AccountBank.LogoUrl
                    },
                    DestAccount = new AccountBankValueObject
                    {
                        Bin = destBankAccount.Bin,
                        AccountNo = destBankAccount.AccountNo,
                        BankName = destBankAccount.BankName,
                        OwnerName = destBankAccount.OwnerName,
                        ShortName = destBankAccount.Bank.ShortName,
                        BankFullName = destBankAccount.Bank.Name,
                        LogoUrl = destBankAccount.Bank.LogoApp
                    },
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = TransactionTypeEnum.Transfer,
                    TransactionStatus = TransactionStatusEnum.Completed,
                    Description = "",
                    TransferContent = body.TransferContent,
                    IsBankingTransfer = true,
                    UseSourceAsLinkingBank = false,
                });

                var transactionDto = _mapper.Map<TransactionDto>(transaction);

                await _transactionProducer.ProduceTransaction(transactionDto);
                return Ok(transactionDto);
            }
            else
            {
                var linkingAccountBank = await _accountExternalService.GetAccountById(body.SourceAccountId)
                ?? throw new AppException("Linking account not found");

                var srcAccountBankNo = linkingAccountBank.AccountBank.BankAccountId;
                var srcBankAccount = await _bankExternalService.GetCitizenAccount(linkingAccountBank.AccountBank.Bin, srcAccountBankNo)
                        ?? throw new AppException("Source bank account not found");

                // prevent source and dest are the same
                if (srcBankAccount.AccountNo.Equals(destBankAccount.AccountNo))
                {
                    throw new AppException("Source bank and destination bank is the same");
                }

                // prevent source don't belong to user
                if (!linkingAccountBank.UserId.Equals(LoggingUserId))
                {
                    throw new AppException("The source account is not yours");
                }

                if (srcBankAccount.Balance < body.TransferAmount)
                {
                    throw new AppException("Balance is not enough to transfer");
                }

                var transaction = _transactionRepo.Insert(new TransactionEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = body.TransferAmount,
                    SourceAccount = new AccountBankValueObject
                    {
                        Bin = srcBankAccount.Bin,
                        AccountNo = srcBankAccount.AccountNo,
                        BankName = srcBankAccount.BankName,
                        OwnerName = srcBankAccount.OwnerName,
                        ShortName = srcBankAccount.Bank.ShortName,
                        BankFullName = srcBankAccount.Bank.Name,
                        LogoUrl = srcBankAccount.Bank.LogoApp
                    },
                    DestAccount = new AccountBankValueObject
                    {
                        Bin = destBankAccount.Bin,
                        AccountNo = destBankAccount.AccountNo,
                        BankName = destBankAccount.BankName,
                        OwnerName = destBankAccount.OwnerName,
                        ShortName = destBankAccount.Bank.ShortName,
                        BankFullName = destBankAccount.Bank.Name,
                        LogoUrl = destBankAccount.Bank.LogoApp
                    },
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = TransactionTypeEnum.Transfer,
                    TransactionStatus = TransactionStatusEnum.Completed,
                    Description = "",
                    TransferContent = body.TransferContent,
                    IsBankingTransfer = true,
                    UseSourceAsLinkingBank = true,
                });



                var transactionDto = _mapper.Map<TransactionDto>(transaction);

                await _transactionProducer.ProduceTransaction(transactionDto);
                return Ok(transactionDto);
            }
        }

        [ServiceFilter(typeof(PinRequiredAttribute))]
        [HttpPost("InternalTransfer")]
        public async Task<IActionResult> InternalTransfer(
           [FromHeader(Name = "X-EncryptedPin")] string pin,
           [FromBody] RequestInternalTransferDto body)
        {
            return Ok();
        }
    }
}
