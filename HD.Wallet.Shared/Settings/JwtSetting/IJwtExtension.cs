using System.Security.Claims;

namespace HD.Wallet.Shared.Settings.JwtSetting
{
    public interface IJwtExtension
    {
        string GenerateToken(string id, string role, string phoneNumber, string email);

        IEnumerable<Claim> DecodeToken(string token);
    }
}
