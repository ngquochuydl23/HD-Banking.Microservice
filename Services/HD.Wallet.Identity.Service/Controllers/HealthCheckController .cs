using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("identity-api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Identity Service is running.");
        }
    }
}
