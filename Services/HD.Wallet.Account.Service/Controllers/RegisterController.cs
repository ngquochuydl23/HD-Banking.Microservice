using AutoMapper;
using HD.Wallet.Account.Service.Dtos;
using HD.Wallet.Account.Service.Dtos.IdCards;
using HD.Wallet.Account.Service.ExternalServices;
using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace HD.Wallet.Account.Service.Controllers
{
    [Route("account-api/[controller]")]
    public class RegisterController : BaseController
    {
        private readonly IEfRepository<UserEntity, string> _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IdCardExternalService _idCardExternalService;

        public RegisterController(
            IEfRepository<UserEntity, string> userRepo,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IdCardExternalService idCardExternalService,
            IMapper mapper) : base(httpContextAccessor)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _idCardExternalService = idCardExternalService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestOpenAccount([FromBody] RequestOpenAccountDto body)
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
            account.TransactionLimit = 10000000;
            account.LinkedAccountId = null;

            var user = new UserEntity();
            user.FullName = body.FullName;
            user.PhoneNumber = body.PhoneNumber;
            user.DateOfBirth = body.DateOfBirth;
            user.Accounts = new HashSet<AccountEntity>() { account };
            user.Email = body.Email;
            user.Sex = 1;
            user.IsEkycVerfied = true;
            user.HashPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(body.Password, 13);
            user.FaceVerificationUrl = body.FaceVerificationUrl;
            user.AccountStatus = UserStatusEnum.Active;
            user.Address = new AddressValueObject()
            {
                Street = body.Address.Street,
                District = body.Address.District,
                ProvinceOrCity = body.Address.ProvinceOrCity,
                WardOrCommune = body.Address.WardOrCommune
            };

            user.Work = new WorkValueObject()
            {
                Occupation = "Lập trình viên",
                Position = "Chuyên viên cao cấp"
            };

            try
            {
                ExternalResponseIdCardDto idCardResponse = await _idCardExternalService.GetIdCardById(body.IdCardNo);
                IdCardDto idCardDetail = idCardResponse.IdCard;

                user.IdCardNo = idCardDetail.Id;
                user.Nationality = idCardDetail.Nationality;
                user.PlaceOfOrigin = idCardDetail.PlaceOfOrigin;
                user.PlaceOfResidence = idCardDetail.PlaceOfResidence;
                
                user.FrontIdCardUrl = idCardResponse.FrontUrl;
                user.BackIdCardUrl = idCardResponse.BackUrl;

                user.IdCardType = idCardResponse.Type;
            }
            catch (Exception ex) { 

            }

            user = _userRepo.Insert(user);
            return Ok(user);
        }
    }
}
