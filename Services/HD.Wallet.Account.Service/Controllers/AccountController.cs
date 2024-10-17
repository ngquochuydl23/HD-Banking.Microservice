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

        [HttpGet("primary")]
        public IActionResult GetPrimaryAccount()
        {
            var account = _accountRepo
                   .GetQueryableNoTracking()
                   .FirstOrDefault(x => !x.IsBankLinking && x.UserId.Equals(LoggingUserId))
                           ?? throw new AppException("Account not found");

            return Ok(account);
        }

        [AllowAnonymous]
        [HttpPost("FindAccount")]
        public IActionResult FindAccount([FromBody] FindAccountDto body)
        {
        
            if (body.IsBankLinking && !string.IsNullOrEmpty(body.BankName))
            {
                var account = _accountRepo
                    .GetQueryableNoTracking()
                    .FirstOrDefault(x => 
                        x.AccountBank.BankAccountId.Equals(body.AccountNo)
                        && x.IsBankLinking
                        && x.AccountBank.BankName.Equals(body.BankName))
                            ?? throw new AppException("Account not found");

                return Ok(account);
            } else
            {

                var account = _accountRepo
                   .GetQueryableNoTracking()
                   .FirstOrDefault(x => x.AccountBank.BankAccountId.Equals(body.AccountNo) && !x.IsBankLinking)
                           ?? throw new AppException("Account not found");

                return Ok(account);

            }
        }

        [HttpGet("Balance")]
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
        public async Task<IActionResult> AddLinkedBankAccount([FromBody] AddLinkedAccountDto body)
        {

            var user = _userRepo.Find(LoggingUserId)
                ?? throw new AppException("User not found");

            if (!user.IdCardNo.Equals(body.IdCardNo))
            {
                throw new AppException("IdCardNo of account and yours is not the same");
            }

            var availableAccount = _accountRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.UserId.Equals(LoggingUserId)
                    && x.AccountBank.BankAccountId.Equals(body.BankAccountId)
                    && x.AccountBank.Bin.Equals(body.Bin));
            if (availableAccount != null)
            {
                throw new AppException("This account bank is already linked");
            }

            try
            {
                BankDto bank = await _bankExternalService.GetBankByBin(body.Bin);

                var account = _accountRepo.Insert(new AccountEntity()
                {
                    UserId = LoggingUserId,
                    IsBankLinking = true,
                    WalletBalance = 0.0,
                    AccountType = AccountTypeEnum.Basic,
                    AccountBank = new AccountBankValueObject()
                    {
                        Bin = body.Bin,
                        BankOwnerName = body.BankOwnerName,
                        BankName = bank.ShortName,
                        BankAccountId = body.BankAccountId,
                        IdCardNo = body.IdCardNo,
                        LogoUrl = bank.LogoApp,
                        BankFullName = bank.ShortName
                    }
                });
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get Bank information");

                throw new AppException("Failed to get Bank information");
            }
        }

        [HttpPost("{accountId}/Blocked")]
        public IActionResult BlockPaymentAccount(string accountId)
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
        public IActionResult UnlinkPaymentAccount(string accountId)
        {
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
    }
}
