using AutoMapper;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Seedworks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HD.Wallet.Account.Service.Controllers
{
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

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
