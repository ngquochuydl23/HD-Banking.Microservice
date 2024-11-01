using AutoMapper;
using HD.Wallet.Account.Service.Dtos;
using HD.Wallet.Account.Service.Dtos.Accounts;
using HD.Wallet.Account.Service.Dtos.IdCards;
using HD.Wallet.Account.Service.ExternalServices;
using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.SharedDtos.Accounts;
using HD.Wallet.Shared.SharedDtos.Banks;
using HD.Wallet.Shared.Utils;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HD.Wallet.Account.Service.Controllers
{
    [Authorize]
    [Route("account-api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IEfRepository<UserEntity, string> _userRepo;
        private readonly IEfRepository<AccountEntity, string> _accountRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BankExternalService _bankExternalService;
        public AccountController(
            IEfRepository<AccountEntity, string> accountRepo,
            IEfRepository<UserEntity, string> userRepo,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            BankExternalService bankExternalService,
            ILogger<AccountController> logger,
            IMapper mapper) : base(httpContextAccessor)
        {
            _accountRepo = accountRepo;
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _bankExternalService = bankExternalService;
        }

        [HttpGet]
        public IActionResult GetAccounts()
        {
            var accounts = _accountRepo
                .GetQueryableNoTracking()
                .Where(x => x.UserId.Equals(LoggingUserId))
                .Where(x => !x.IsUnlinked)
                .OrderBy(x => x.IsBankLinking)
                .ToList();

            return Ok(accounts);
        }


        [AllowAnonymous]
        [HttpGet("{accountId}")]
        public IActionResult GetAccountById(string accountId)
        {

            var account = _accountRepo
                   .GetQueryableNoTracking()
                   .FirstOrDefault(x => x.Id.Equals(accountId))
                           ?? throw new AppException("Account not found");

            return Ok(account);
        }

        [HttpGet("Primary")]
        public IActionResult GetPrimaryAccount()
        {
            var account = _accountRepo
                   .GetQueryableNoTracking()
                   .FirstOrDefault(x => !x.IsBankLinking && x.UserId.Equals(LoggingUserId))
                           ?? throw new AppException("Account not found");

            return Ok(account);
        }


        [AllowAnonymous]
        [HttpGet("Wallet/{accountNo}")]
        public IActionResult GetWalletAccountByNo(string accountNo)
        {
            var account = _accountRepo
                   .GetQueryableNoTracking()
                   .FirstOrDefault(x => !x.IsBankLinking && x.AccountBank.BankAccountId.Equals(accountNo))
                           ?? throw new AppException("Account not found");

            return Ok(account);
        }

        [HttpGet("Wallet/Balance")]
        public IActionResult GetAccountBalance()
        {
            var account = _accountRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.UserId.Equals(LoggingUserId))
                    ?? throw new AppException("Account not found");

            if (account.IsBankLinking)
            {
                throw new AppException("Cannot track bank account");
            }

            return Ok(new
            {
                Balance = account.WalletBalance,
                AccountId = account.Id
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddLinkedBankAccount(
            [FromBody] AddLinkedAccountDto body,
            [FromHeader(Name = "X-EncryptedPin")] string encryptedPin)
        {
          
            var user = _userRepo.Find(LoggingUserId)
                ?? throw new AppException("User not found");

            ValidatePinLocal(encryptedPin, user.PinPassword);

            BankDto bank = await _bankExternalService.GetBankByBin(body.Bin)
                ?? throw new AppException("Bank not found with Bin:" + body.Bin);

            CitizenAccountDto citizenAccount = await _bankExternalService.GetCitizenAccount(body.Bin, body.BankAccountId)
                ?? throw new AppException("AccountBank not found");

            if (!citizenAccount.IdCardNo.Equals(body.IdCardNo))
            {
                throw new AppException("Provided IdCard is not the same of bank account");
            }

            if (!user.IdCardNo.Equals(body.IdCardNo))
            {
                throw new AppException("IdCardNo of account and yours is not the same");
            }

            if (!citizenAccount.Status.Equals("Active"))
            {
                throw new AppException("Your bank account status is not active");
            }

            var availableLikingAccount = _accountRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.UserId.Equals(LoggingUserId)
                    && x.AccountBank.BankAccountId.Equals(body.BankAccountId)
                    && x.AccountBank.Bin.Equals(body.Bin));

            if (availableLikingAccount != null)
            {
                throw new AppException("This account bank is already linked");
            }

            var account = _accountRepo.Insert(new AccountEntity()
            {
                Id = citizenAccount.AccountNo,
                UserId = LoggingUserId,
                IsBankLinking = true,
                WalletBalance = citizenAccount.Balance,
                AccountType = AccountTypeEnum.Basic,
                AccountBank = new AccountBankValueObject()
                {
                    Bin = body.Bin,
                    BankOwnerName = citizenAccount.OwnerName,
                    BankName = bank.ShortName,
                    BankAccountId = citizenAccount.AccountNo,
                    IdCardNo = citizenAccount.IdCardNo,
                    LogoUrl = bank.LogoApp,
                    BankFullName = bank.ShortName
                }
            });


            return Ok(_mapper.Map<AccountDto>(account));
        }

        [HttpPost("{accountId}/Blocked")]
        public IActionResult BlockPaymentAccount(
            string accountId,
            [FromHeader(Name = "X-EncryptedPin")] string encryptedPin)
        {
            var account = _accountRepo
                .GetQueryable()
                .FirstOrDefault(x => x.Id.Equals(accountId) && x.UserId.Equals(LoggingUserId))
                    ?? throw new AppException("Account not found");

            if (account.IsBlocked)
            {
                throw new AppException("Account is already blocked before.");
            }

            account.IsBlocked = true;
            account = _accountRepo.Update(accountId, account);

            return Ok(account);
        }

        [HttpPost("{accountId}/Unlink")]
        public IActionResult UnlinkPaymentAccount(
            string accountId,
            [FromHeader(Name = "X-EncryptedPin")] string encryptedPin)
        {
            var user = _userRepo.Find(LoggingUserId)
                ?? throw new AppException("User not found");

            ValidatePinLocal(encryptedPin, user.PinPassword);

            var account = _accountRepo
                .GetQueryable()
                .FirstOrDefault(x => x.Id.Equals(accountId) 
                    && x.UserId.Equals(LoggingUserId))
                    ?? throw new AppException("Account not found");

            if (!account.IsBankLinking)
            {
                throw new AppException("Cannot remove non-linking account");
            }

            if (account.IsUnlinked)
            {
                throw new AppException("Account is already unlinked before.");
            }

            account.IsUnlinked = true;
            account = _accountRepo.Update(accountId, account);

            return Ok(account);
        }

        private bool ValidatePinLocal(string encryptedPin, string userPinHash)
        {
            if (!Base64Validator.IsBase64String(encryptedPin))
            {
                throw new AppException("EncryptedPin is invalid");
            }

            var pin = AesDecryption.Decrypt(encryptedPin);
            if (!BCrypt.Net.BCrypt.Verify(pin, userPinHash))
            {
                throw new AppException("Pin is incorrect");
            }
            return true;
        }
    }
}
