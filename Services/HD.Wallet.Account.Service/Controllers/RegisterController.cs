using AutoMapper;
using HD.Wallet.Account.Service.Dtos;
using HD.Wallet.Account.Service.Dtos.IdCards;
using HD.Wallet.Account.Service.ExternalServices;
using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Account.Service.Validators;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace HD.Wallet.Account.Service.Controllers
{
    [Route("account-api/[controller]")]
    public class RegisterController : BaseController
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IEfRepository<UserEntity, string> _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IdCardExternalService _idCardExternalService;
        private readonly RequestOpenAccountValidator _validations;
        public RegisterController(
            IEfRepository<UserEntity, string> userRepo,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IdCardExternalService idCardExternalService,
            ILogger<RegisterController> logger,
            RequestOpenAccountValidator validations,
            IMapper mapper) : base(httpContextAccessor)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validations = validations;
            _idCardExternalService = idCardExternalService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestOpenAccount([FromBody] RequestOpenAccountDto body)
        {   
            var validationResult = await _validations.ValidateAsync(body);
            if (!validationResult.IsValid)
            {
                throw new AppException(validationResult.Errors.ToString());
            }

            var user = new UserEntity();
            var availableUser = _userRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.IdCardNo.Equals(body.PhoneNumber)
                    || x.Email.Equals(body.Email)
                    || x.IdCardNo.Equals(body.IdCardNo));
            
            if (availableUser != null)
            {
                var errors = new List<object>();

                foreach (var failure in validationResult.Errors)
                {
                    errors.Add(new
                    {
                        Field = failure.PropertyName,
                        Error = failure.ErrorMessage
                    });
                }
                throw new AppException(JsonConvert.SerializeObject(errors));
            }

            try
            {
                ExternalResponseIdCardDto idCardResponse = await _idCardExternalService.GetIdCardById(body.IdCardNo);
                IdCardDto idCardDetail = idCardResponse.IdCard;


                user.FullName = body.FullName;
                user.PhoneNumber = body.PhoneNumber;
                user.DateOfBirth = body.DateOfBirth;
                user.Accounts = new HashSet<AccountEntity>() {
                    new AccountEntity()
                    {
                        IsBankLinking = false,
                        WalletBalance = 0.0,
                        AccountType = AccountTypeEnum.Basic,
                        AccountBank = new AccountBankValueObject()
                        {
                            BankFullName = "Ví điện tử HDWallet",
                            BankOwnerName = AccountBankValueObject.ToUpperCaseWithoutDiacritics(body.FullName),
                            BankName = "HD_WALLET_MBBANK",
                            BankAccountId = body.PhoneNumber,
                            LogoUrl = "",
                            IdCardNo = body.IdCardNo,
                            Bin = "9999.0"
                        },
                        TransactionLimit = 10000000,
                        LinkedAccountId = null,
                      
                    }
                };

                user.Email = body.Email;
                user.Sex = idCardDetail.Sex.Equals("Nữ") ? 0 : 1;
                user.IsEkycVerfied = true;
                user.HashPassword = BCrypt.Net.BCrypt.HashPassword(body.Password);
                user.PinPassword = BCrypt.Net.BCrypt.HashPassword("111111");
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

                user.IdCardNo = idCardDetail.Id;
                user.Nationality = idCardDetail.Nationality;
                user.PlaceOfOrigin = idCardDetail.PlaceOfOrigin;
                user.PlaceOfResidence = idCardDetail.PlaceOfResidence;
                user.FrontIdCardUrl = idCardResponse.FrontUrl;
                user.BackIdCardUrl = idCardResponse.BackUrl;
                user.IdCardType = idCardResponse.Type;

                user = _userRepo.Insert(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get IdCard information");

                throw new AppException("Failed to get IdCard information");
            }
        }
    }
}
