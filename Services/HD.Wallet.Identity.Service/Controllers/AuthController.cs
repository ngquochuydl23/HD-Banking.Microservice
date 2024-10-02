using HD.Wallet.Identity.Service.Dtos;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HD.Wallet.Identity.Service.Controllers
{
    [Route("identity-api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> Post([FromBody] RequestLoginDto body)
        {
 
            var tokenResponse = await RequestToken(body.PhoneNumber, body.Password);
            
            if (tokenResponse.IsError)
            {
                return BadRequest(tokenResponse.Error);
            }

            return Ok(tokenResponse);
        }

        private async Task<TokenResponse> RequestToken(string username, string password)
        {
            // Discover endpoints from the IdentityServer
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync("http://localhost:8300"); // IdentityServer URL

            if (discoveryDocument.IsError)
            {
                throw new HttpRequestException(discoveryDocument.Error);
            }

            // 

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1")


            return tokenResponse;
        }
    }
}
