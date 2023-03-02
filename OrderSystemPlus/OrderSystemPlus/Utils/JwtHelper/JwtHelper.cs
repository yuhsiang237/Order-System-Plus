using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace OrderSystemPlus.Utils.JwtHelper
{
    public class JwtHelper : IJwtHelper
    {
        private IConfiguration _configuration;
        private readonly string _issuer;
        private readonly string _signKey;
        private readonly int _expireMinutes;
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _signKey = _configuration.GetValue<string>("JwtSettings:SignKey");
            _issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
            _expireMinutes = _configuration.GetValue<int>("JwtSettings:ExpireMinutes");
        }
        public string GenerateToken(string account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Add Token Desc
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Subject = new ClaimsIdentity(new[]
                {
                  new Claim(JwtRegisteredClaimNames.Sub, account), // User identity Key
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                }),
                Expires = DateTime.Now.AddMinutes(_expireMinutes),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return serializeToken;
        }
    }
}