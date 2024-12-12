using AutoMapper;
using HD.Wallet.Account.Service.Dtos;
using HD.Wallet.Account.Service.Infrastructure.Entities.Otps;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HD.Wallet.Account.Service.Controllers
{
    [Route("account-api/[controller]")]
    public class OtpController : BaseController
    {
        private readonly IEfRepository<OtpEntity, long> _otpRepo;
        private readonly IEfRepository<UserEntity, string> _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OtpController(
            IEfRepository<UserEntity, string> userRepo,
            IEfRepository<OtpEntity, long> otpRepo,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(httpContextAccessor)
        {
            _userRepo = userRepo;
            _otpRepo = otpRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost("RequestProvideOtp")]
        public IActionResult RequestProvideOtp([FromBody] RequestProvideOtpDto body)
        {
            if (string.IsNullOrEmpty(body.Key))
            {
                throw new AppException("body.Key must not be empty or null");
            }

            //var otp = Otp.GenerateOtp(6);
            var otp = "111111";


            if (_otpRepo
                .GetQueryableNoTracking()
                .Any(x => x.Key.Equals(body.Key) && !x.Verified && x.Type.Equals(body.Type) && x.ExpiredAt >= DateTime.UtcNow))
            {
                throw new AppException("You've already requested providing otp code.");
            }

            var otpObj = _otpRepo.Insert(new OtpEntity()
            {
                Key = body.Key,
                Type = body.Type,
                OtpCode = otp,
                ExpiredAt = DateTime.UtcNow.AddMinutes(1),
                Verified = false
            });

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, body.Key),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("type", body.Type),
                new Claim("otpId", otpObj.Id.ToString())
            };

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("e3b36aed-8792-442f-9d74-599f0b8cdedf\r\n")), SecurityAlgorithms.HmacSha256)
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(jwtToken);

            return Ok(new
            {
                OtpId = otpObj.Id,
                Token = token,
                Type = body.Type,
                ExpiredAt = DateTime.UtcNow.AddMinutes(1),
            });
        }


        [HttpPost("VerifyOtp")]
        public IActionResult VerifyPhoneNumber(
           [FromHeader(Name = "Token")] string token,
           [FromBody] RequestVerifyOtpDto body)
        {

            if (string.IsNullOrEmpty(body.OtpCode))
            {
                throw new AppException("body.OtpCode must not be empty or null");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            if (jwtSecurityToken.ValidTo < DateTime.UtcNow)
            {
                throw new AppException("Token has expired.");
            }

            var phoneNumber = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var type = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "type")?.Value;
            var otpId = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "otpId")?.Value;


            var otpObj = _otpRepo
                .GetQueryable()
                .FirstOrDefault(x => !x.Verified && x.Id == long.Parse(otpId))
                    ?? throw new AppException("Otp not found");

            if (!otpObj.OtpCode.Equals(body.OtpCode))
            {
                throw new AppException("Otp is incorrect");
            }

            otpObj.Verified = true;


            _otpRepo.SaveChanges();

            return Ok(new
            {
                Msg = "OK"   
            });
        }
    }
}
