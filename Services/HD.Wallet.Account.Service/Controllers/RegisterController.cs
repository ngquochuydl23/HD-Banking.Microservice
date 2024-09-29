using AutoMapper;
using HD.Wallet.Account.Service.Dtos;
using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using Microsoft.AspNetCore.Mvc;

namespace HD.Wallet.Account.Service.Controllers
{
    public class RegisterController : BaseController
    {
        private readonly IEfRepository<UserEntity, string> _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterController(
            IEfRepository<UserEntity, string> userRepo,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(httpContextAccessor)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpPost]
        public IActionResult RequestOpenAccount([FromBody] RequestOpenAccountDto body)
        {

            var account = new AccountEntity();

            account.IsBankLinking = false;
            account.WalletBalance = 0.0;
            account.AccountType = AccountTypeEnum.Basic;
            account.AccountBank = new AccountBankValueObject()
            {
                BankOwnerName = AccountBankValueObject.ToUpperCaseWithoutDiacritics(body.FullName),
                BankName = "HD_WALLET_MBBANK",
                BankAccountId = body.PhoneNumber,
                IdCardNo = body.IdCardNo,
            };

            ICollection<AccountEntity> accounts = new HashSet<AccountEntity>();
            accounts.Add(account);


            var user = new UserEntity();
            user.FullName = "Nguyễn Quốc Huy";
            user.PhoneNumber = "0868684961";
            user.Accounts = accounts;
            user.Email = "nguyenquochuydl123@gmail.com";
            user.Sex = 1;
           

            return Ok(user);
        }
    }
}
