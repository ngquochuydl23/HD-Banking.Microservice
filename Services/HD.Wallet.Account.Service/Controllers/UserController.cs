using AutoMapper;
using HD.Wallet.Account.Service.Dtos;
using HD.Wallet.Account.Service.Dtos.Users;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HD.Wallet.Account.Service.Controllers
{
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

        [HttpPost("validate")]
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

        [HttpGet("find-user-by-phone")]
        public async Task<IActionResult> FindUserByPhone([FromQuery] string phone)
        {
            var user = _userRepo
              .GetQueryable()
              .FirstOrDefault(x => x.PhoneNumber.Equals(phone))
                ?? throw new AppException("User not found");


            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}
