using AutoMapper;
using HD.Wallet.Account.Service.Dtos;
using HD.Wallet.Account.Service.Dtos.Users;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace HD.Wallet.Account.Service.Controllers
{
    [Authorize]
    [Route("account-api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IEfRepository<UserEntity, string> _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(
            IEfRepository<UserEntity, string> userRepo,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(httpContextAccessor)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("UserInfo")]
        public IActionResult GetUserInfo()
        {
            var user = _userRepo
                 .GetQueryable()
                 .FirstOrDefault(x => x.Id.Equals(LoggingUserId))
                   ?? throw new AppException("User not found");

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPut("UserInfo")]
        public IActionResult UpdateUserInfo([FromBody] RequestUpdateUserInfoDto body)
        {
            var user = _userRepo
                 .GetQueryableNoTracking()
                 .FirstOrDefault(x => x.Id.Equals(LoggingUserId))
                   ?? throw new AppException("User not found");

            return Ok(_mapper.Map<UserDto>(user));
        }

        [AllowAnonymous]
        [HttpPost("Validate")]
        public async Task<IActionResult> ValidateUser([FromBody] RequestValidateUserDto body)
        {
            var user = _userRepo
                .GetQueryable()
                .FirstOrDefault(x => x.PhoneNumber.Equals(body.PhoneNumber))
                ?? throw new AppException("User not found");


            if (!BCrypt.Net.BCrypt.Verify(body.Password, user.HashPassword))
            {
                throw new AppException("Password is incorrect");
            }

            return Ok(_mapper.Map<UserDto>(user));

        }

        [AllowAnonymous]
        [HttpGet("FindUserByPhone")]
        public async Task<IActionResult> FindUserByPhone([FromQuery] string phone)
        {
            var user = _userRepo
                .GetQueryable()
                .FirstOrDefault(x => x.PhoneNumber.Equals(phone))
                ?? throw new AppException("User not found");


            return Ok(_mapper.Map<UserDto>(user));
        }

        [Authorize]
        [HttpPost("ValidatePinNumber")]
        public IActionResult ValidatePinNumber([FromHeader(Name = "X-EncryptedPin")] string encryptedPin)
        {
            var user = _userRepo
                .GetQueryableNoTracking()
                .FirstOrDefault(x => x.Id.Equals(LoggingUserId))
                    ?? throw new AppException("User not found");


            if (!Base64Validator.IsBase64String(encryptedPin))
            {
                throw new AppException("EncryptedPin is invalid");
            }
         
            var pin = AesDecryption.Decrypt(encryptedPin);
            if (!BCrypt.Net.BCrypt.Verify(pin, user.PinPassword))
            {
                throw new AppException("Pin is incorrect");
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("ChangePinPassword")]
        public IActionResult SetUpPinPassword(
            [FromHeader(Name = "X-EncryptedPin")] string encryptedPin,
            [FromBody] RequestChangePinPassword body
            )
        {
            var user = _userRepo
                .GetQueryable()
                .FirstOrDefault(x => x.Id.Equals(LoggingUserId))
                    ?? throw new AppException("User not found");


            if (!Base64Validator.IsBase64String(encryptedPin))
            {
                throw new AppException("EncryptedPin is invalid");
            }

            var pin = AesDecryption.Decrypt(encryptedPin);
            if (!BCrypt.Net.BCrypt.Verify(pin, user.PinPassword))
            {
                throw new AppException("Pin is incorrect");
            } 

            user.PinPassword = BCrypt.Net.BCrypt.HashPassword(body.NewEncryptedPin);

            _userRepo.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpPost("UpdateAvatar")]
        public IActionResult UpdateAvatar([FromBody] RequestUpdateAvatarDto body)
        {
            if (string.IsNullOrEmpty(body.Avatar))
            {
                throw new AppException("Avatar must not be null of empty.");
            }

            var user = _userRepo
                .GetQueryable()
                .FirstOrDefault(x => x.Id.Equals(LoggingUserId))
                    ?? throw new AppException("User not found");

            user.Avatar = body.Avatar;
            _userRepo.SaveChanges();

            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}
