using HD.Wallet.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HD.Wallet.BankingResource.Service.Controllers
{
    [Route("banking-resource-api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly ILogger<UploadsController> _logger;    

        public UploadsController(ILogger<UploadsController> logger)
        {
            _logger = logger;
        }


        [HttpGet("{fileName}")]
        public IActionResult GetLogoByName(string fileName)
        {

            try
            {
                var b = System.IO.File.ReadAllBytes(@"Data/BankingLogos/" + fileName);
                return File(b, "image/png");
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message, ex);
                throw new AppException("Image not found");
            }
        }
    }
}
