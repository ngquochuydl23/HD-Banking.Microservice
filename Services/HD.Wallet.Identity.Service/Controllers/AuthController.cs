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
            // Validate the login request
            if (body == null || string.IsNullOrEmpty(body.PhoneNumber) || string.IsNullOrEmpty(body.Password))
            {
                return BadRequest("Invalid login request.");
            }

            // Request token from IdentityServer
            var tokenResponse = await RequestToken(body.PhoneNumber, body.Password);
            
            if (tokenResponse.IsError)
            {
                return BadRequest(tokenResponse.Error); // Return error from IdentityServer
            }

            return Ok(tokenResponse.AccessToken); // Return JWT token
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

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "local-dev", // The client_id from Config.cs
                ClientSecret = "yoursecretvalue",
                Scope = "account transaction" // Adjust scopes based on your configuration
            });


            return tokenResponse;
        }
    }
}
