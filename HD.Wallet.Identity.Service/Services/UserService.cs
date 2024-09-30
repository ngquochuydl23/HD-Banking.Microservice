using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;

namespace HD.Wallet.Identity.Service.Services
{
    public class UserService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.FindFirst(JwtClaimTypes.Subject)?.Value; // Get the user ID from the claims
            context.IssuedClaims.Add(new Claim("user_id", userId)); // Add user ID to the token claims

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true; // Set to true if the user is active
            return Task.CompletedTask;
        }
    }
}
