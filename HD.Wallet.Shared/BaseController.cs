using HD.Wallet.Shared.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor 
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpContext = httpContextAccessor.HttpContext;
        }


        public override OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            return base.Ok(new
            {
                Result = value,
				StatusCode = StatusCodes.Status200OK
            });
        }


        protected string AccessToken => Request.Headers["Authorization"].ToString();
        protected string LoggingUserId => _httpContext.User.FindFirstValue("id");
        protected string Role => _httpContext.User.FindFirstValue("role");
        protected string PhoneNumber => _httpContext.User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone");

    }
}
