using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace OrderSystemPlus.Utils.JwtHelper
{
    public class JwtHelper : IJwtHelper
    {
        private readonly string secretKey;
        private IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            secretKey = _configuration.GetValue<string>("JwtSettings:SecretKey");
        }


        // 生成 Access Token
        public string GenerateAccessToken(string userId, string userAccount, int expiresInMinutes)
        {
            // 设置 Token 的有效期
            DateTime expires = DateTime.Now.AddMinutes(expiresInMinutes);

            // 设置 Token 的身份信息（用户ID和用户名等）
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userAccount),
                // 添加其他自定义的 Claim
            };

            // 创建 JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires =  expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // 生成 Refresh Token
        public string GenerateRefreshToken(string userId, string userAccount, int expiresInDays = 14)
        {
            // 设置 Token 的有效期
            DateTime expires = DateTime.UtcNow.AddDays(expiresInDays);

            // 设置 Token 的身份信息（用户ID和用户名等）
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userAccount),
            // 添加其他自定义的 Claim
        };

            // 创建 JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}