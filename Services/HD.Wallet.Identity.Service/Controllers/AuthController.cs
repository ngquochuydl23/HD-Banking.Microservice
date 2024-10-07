using AutoMapper;
using HD.Wallet.Identity.ExternalServices;
using HD.Wallet.Identity.Service.Dtos;
using HD.Wallet.Shared;
using HD.Wallet.Shared.Seedworks;
using HD.Wallet.Shared.Settings.JwtSetting;
using IdentityModel.Client;
using IdentityServer4.Models;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using static IdentityServer4.Models.IdentityResources;


namespace HD.Wallet.Identity.Service.Controllers
{
    [Route("identity-api/[controller]")]
    public class AuthController : BaseController
    {

        private readonly IJwtExtension _jwtExtension;
        private readonly UserExternalService _userExternalService;

        public AuthController(
            IJwtExtension jwtExtension,
            IHttpContextAccessor httpContextAccessor,
            UserExternalService userExternalService) : base(httpContextAccessor)
        {
            _jwtExtension = jwtExtension;
            _userExternalService = userExternalService;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] RequestLoginDto body)
        {
            var user = await _userExternalService.ValidateUser(body.PhoneNumber, body.Password);
            var token = _jwtExtension.GenerateToken(user.Id, "User", user.PhoneNumber, user.Email);

            return Ok(new { user, token });
        }
    }
}
