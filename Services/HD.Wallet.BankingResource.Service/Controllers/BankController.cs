using AutoMapper;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Seedworks;
using Microsoft.AspNetCore.Mvc;

namespace HD.Wallet.BankingResource.Service.Controllers
{
    [ApiController]
    [Route("banking-resource-api/[controller]")]
    public class BankController : BaseController
    {

        private readonly ILogger<BankController> _logger;

        public BankController(
            IHttpContextAccessor httpContextAccessor, 
            IMapper mapper, 
            ILogger<BankController> logger) : base(httpContextAccessor)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetBanks()
        {
            return Ok();
        }
    }
}
