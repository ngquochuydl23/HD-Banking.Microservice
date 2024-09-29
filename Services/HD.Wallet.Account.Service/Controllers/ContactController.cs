using AutoMapper;
using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared;
using Microsoft.AspNetCore.Mvc;


namespace HD.Wallet.Account.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : BaseController
    {

        private readonly IEfRepository<UserEntity, string> _userRepo;
        private readonly IEfRepository<AccountEntity, string> _accountRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactController(
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ContactController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ContactController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ContactController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ContactController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
