using System.Security.Claims;

namespace HD.Wallet.Shared.Settings.JwtSetting
{
    public interface IJwtExtension
    {
        string GenerateToken(long id, string role);

        IEnumerable<Claim> DecodeToken(string token);
    }
}
