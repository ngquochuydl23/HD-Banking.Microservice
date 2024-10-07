using AutoMapper;
using HD.Wallet.Account.Service.Dtos;
using HD.Wallet.Account.Service.Dtos.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HD.Wallet.Account.Service.Controllers
{
    [Authorize]
    [Route("account-api/[controller]")]
    public class AccountController : BaseController
    {

        private readonly IEfRepository<UserEntity, string> _userRepo;
        private readonly IEfRepository<AccountEntity, string> _accountRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountController(
            IEfRepository<AccountEntity, string> accountRepo,
            IEfRepository<UserEntity, string> userRepo,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(httpContextAccessor)
        {
            _accountRepo = accountRepo;
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        [HttpGet("{bankAccountNo}")]
        public IActionResult GetAccountByBankNo(string bankAccountNo)
        {
            var account = _accountRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.AccountBank.BankAccountId.Equals(bankAccountNo) && x.UserId.Equals(LoggingUserId))
                    ?? throw new AppException("Account not found");

            return Ok(account);
        }

        [HttpGet("balance")]
        public IActionResult GetAccountBalance()
        {
            var account = _accountRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => !x.IsBankLinking && x.UserId.Equals(LoggingUserId))
                    ?? throw new AppException("Account not found");

            return Ok(new
            {
                Balance = account.WalletBalance,
                AccountId = account.Id
            });
        }

        [HttpPost]
        public IActionResult AddLinkedBankAccount([FromBody] AddLinkedAccountDto body)
        {

            var user = _userRepo.Find(LoggingUserId)
                ?? throw new AppException("User not found");

            if (!user.IdCardNo.Equals(body.IdCardNo))
            {
                throw new AppException("IdCardNo of account and yours is not the same");
            }

            var account = _accountRepo.Insert(new AccountEntity()
            {
                UserId = LoggingUserId,
                IsBankLinking = true,
                WalletBalance = 0.0,
                AccountType = AccountTypeEnum.Basic,
                AccountBank = new AccountBankValueObject()
                {
                    BankOwnerName = body.BankOwnerName,
                    BankName = body.BankName,
                    BankAccountId = body.BankAccountId,
                    IdCardNo = body.IdCardNo,
                }
            });
            return Ok(account);
        }

        [HttpPost("{accountId}/blocked")]
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

        [HttpPost("{accountId}/unlink")]
        public IActionResult UnlinkPaymentAccount(string accountId)
        {
            var account = _accountRepo
                .GetQueryable()
                .FirstOrDefault(x => x.Id.Equals(accountId) && x.UserId.Equals(LoggingUserId))
                    ?? throw new AppException("Account not found");

            account.IsUnlinked = true;

            if (account.IsUnlinked)
            {
                throw new AppException("Account is already blocked before.");
            }

            account = _accountRepo.Update(accountId, account);

            return Ok(account);
        }

        [HttpPost("main/generate-qr-code")]
        public IActionResult GenerateQrCode()
        {
            return Ok();
        }
    }
}
