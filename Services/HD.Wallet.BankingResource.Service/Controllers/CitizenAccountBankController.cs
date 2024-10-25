using AutoMapper;
using HD.Wallet.BankingResource.Service.Infrastructure;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.SharedDtos.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HD.Wallet.BankingResource.Service.Controllers
{
    [ApiController]
    [Route("banking-resource-api/[controller]")]
    public class CitizenAccountBankController : BaseController
    {
        private readonly ILogger<CitizenAccountBankController> _logger;
        private readonly BankingResourceDbContext _dbContext;
        private readonly IMapper _mapper;

        public CitizenAccountBankController(
            IHttpContextAccessor httpContextAccessor,
            BankingResourceDbContext dbContext,
            IMapper mapper,
            ILogger<CitizenAccountBankController> logger) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetCitizenAccountBank([FromQuery] string bin, [FromQuery] string accountNo)
        {
            var citizenAccount = _dbContext.CitizenAccountBanks
                .AsNoTracking()
                .Include(x => x.Bank)
                .FirstOrDefault(x => x.AccountNo.Equals(accountNo) && x.Bin.Equals(bin))
                    ?? throw new AppException("Citizen Account not found.");

            return Ok(_mapper.Map<CitizenAccountDto>(citizenAccount));
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
