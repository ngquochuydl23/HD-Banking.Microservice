using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace HD.Wallet.Identity.Service
{
    public class CustomProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // Add custom claims here
            var claims = new List<Claim>
            {
                new Claim("iduser", "1234567"),
                new Claim(ClaimTypes.Role, "admin")
            };


            context.IssuedClaims.AddRange(claims);
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;  // Determine if the user/client is active
            return Task.CompletedTask;
        }
    }
}
