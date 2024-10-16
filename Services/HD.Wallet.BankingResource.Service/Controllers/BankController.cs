using AutoMapper;
using HD.Wallet.BankingResource.Service.Services;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.SharedDtos.Banks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HD.Wallet.BankingResource.Service.Controllers
{
    [ApiController]
    [Route("banking-resource-api/[controller]")]
    public class BankController : BaseController
    {

        private readonly ILogger<BankController> _logger;
        private readonly IServiceCsvLoader _csvLoader;
        public BankController(
            IHttpContextAccessor httpContextAccessor, 
            IServiceCsvLoader csvLoader,
            ILogger<BankController> logger) : base(httpContextAccessor)
        {
            _logger = logger;
            _csvLoader = csvLoader;
        }

        [HttpGet]
        public IActionResult GetBanks()
        {
            List<BankDto> banks = _csvLoader.GetAllBanks();
            return Ok(banks);
        }

        [HttpGet("top")]
        public IActionResult GetTopBanks()
        {
            List<BankDto> banks = _csvLoader.GetTopBanks();
            return Ok(banks);
        }


        [HttpGet("{bin}")]
        public IActionResult GetBankByBin(string bin)
        {
            BankDto bank = _csvLoader.GetBankByBin(bin)
                ?? throw new AppException("Bank not found"); 

            return Ok(bank);
        }
    }
}
