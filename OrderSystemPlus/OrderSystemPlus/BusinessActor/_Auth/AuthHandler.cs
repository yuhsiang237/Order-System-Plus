using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.Utils.HashSaltTool;
using OrderSystemPlus.Utils.JwtHelper;

namespace OrderSystemPlus.BusinessActor
{
    public class AuthHandler : IAuthHandler
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserRepository _userRepository;
        private readonly string secretKey;
        private IConfiguration _configuration;
        public AuthHandler(
            IUserRepository userRepository,
            IJwtHelper jwtHelper,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
            secretKey = _configuration.GetValue<string>("JwtSettings:SecretKey");
        }

        public async Task<(string AccessToken, string RefreshToken)> HandleAsync(ReqSignIn req)
        {
            var user = (await _userRepository.FindByOptionsAsync(null, null, req.Account))
                .FirstOrDefault() ?? new UserDto();
            var isValid = HashSaltTool.Validate(req.Password,
                                                    user.Salt,
                                                    user.Password);
            if (isValid)
            {
                var refreshToken = _jwtHelper.GenerateRefreshToken(user.Id.ToString(), user.Account);
                var accessToken = _jwtHelper.GenerateAccessToken(user.Id.ToString(), user.Account, 1);

                return (accessToken, refreshToken);
            }
            else
            {
                throw new BusinessException("登入失敗，請確認帳號密碼是否正確。");
            }
        }

        public async Task<RspRefreshAccessToken> HandleAsync(ReqRefreshAccessToken req)
        {
            try
            {
                var refreshToken = req.RefreshToken;
                var tokenHandler = new JwtSecurityTokenHandler();

                // // Validate conditions
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                // Validate token
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);

                string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                string username = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var accessToken = _jwtHelper.GenerateAccessToken(userId, username, 1);

                return await Task.FromResult(new RspRefreshAccessToken
                {
                    AccessToken = accessToken
                });
            }
            catch (Exception)
            {
                return await Task.FromResult(new RspRefreshAccessToken
                {
                    AccessToken = string.Empty,
                });
            }
        }

        public async Task<bool> HandleAsync(ReqValidateAccessToken req)
        {
            try
            {
                var accessToken = req.AccessToken;

                // Validate conditions
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                // Validate token
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);

                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
        }
    }
}
