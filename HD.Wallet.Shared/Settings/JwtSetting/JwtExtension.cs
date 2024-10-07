using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HD.Wallet.Shared.Settings.JwtSetting
{
    public class JwtExtension : IJwtExtension
    {
        private readonly JwtSettings _jwtSettings;

        public JwtExtension(IOptions<JwtSettings> jwtSettingOptions)
        {
            _jwtSettings = jwtSettingOptions.Value;
        }

        public IEnumerable<Claim> DecodeToken(string token)
        {
            var obj = new JwtSecurityToken(token);
            return obj.Claims;
        }

        public string GenerateToken(string id, string role, string phoneNumber, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var signingCredentials = new SigningCredentials(
              new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
              SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", id),
                    new Claim(ClaimTypes.MobilePhone, phoneNumber),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
