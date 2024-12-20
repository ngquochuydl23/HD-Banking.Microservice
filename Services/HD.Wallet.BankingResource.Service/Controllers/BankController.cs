using AutoMapper;
using HD.Wallet.BankingResource.Service.Infrastructure;
using HD.Wallet.BankingResource.Service.Infrastructure.Entities;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Queries;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.SharedDtos.Banks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace HD.Wallet.BankingResource.Service.Controllers
{
    [ApiController]
    [Route("banking-resource-api/[controller]")]
    public class BankController : BaseController
    {

        private readonly ILogger<BankController> _logger;
        private readonly BankingResourceDbContext _dbContext;
        private readonly IMapper _mapper;

        public BankController(
            IHttpContextAccessor httpContextAccessor,
            BankingResourceDbContext dbContext,
            IMapper mapper,
            ILogger<BankController> logger) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetBanks([FromQuery] string? search)
        {
            var banks = _dbContext.Banks
                .AsNoTracking()
                .AsNoTracking()
                .Where(x => !x.Bin.Equals("999999.0"))
                .Where(b =>
                    EF.Functions.Like(b.Code, $"%{search}%") ||
                    EF.Functions.Like(b.Name, $"%{search}%") ||
                    EF.Functions.Like(b.ShortName, $"%{search}%"))
                .ToList();

            return Ok(_mapper.Map<List<BankDto>>(banks));
        }

        [HttpGet("top")]
        public IActionResult GetTopBanks()
        {
            var banks = _dbContext.Banks
               .AsNoTracking()
               .Where(x => !x.Bin.Equals("999999.0"))
               .OrderBy(x => x.Top)
               .ToList();

            return Ok(_mapper.Map<List<BankDto>>(banks));
        }


        [HttpGet("{bin}")]
        public IActionResult GetBankByBin(string bin)
        {
            var bank = _dbContext.Banks
                  .AsNoTracking()
                  .FirstOrDefault(x => x.Bin.Equals(bin))
                        ?? throw new AppException("Bank not found");
            return Ok(_mapper.Map<BankDto>(bank));
        }
    }
}
