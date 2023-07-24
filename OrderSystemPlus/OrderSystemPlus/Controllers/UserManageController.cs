using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Utils.JwtHelper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserManageController : ControllerBase
    {
        private readonly IUserManageHandler _userHandler;
        private readonly string secretKey;
        private IConfiguration _configuration;
        private readonly IJwtHelper _jwtHelper;

        public UserManageController(
            IUserManageHandler userHandler,
            IConfiguration configuration,
            IJwtHelper jwtHelper)
        {
            _userHandler = userHandler;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
            secretKey = _configuration.GetValue<string>("JwtSettings:SecretKey");
        }

        [HttpPost("CreateUser")]
        public async Task<int> CreateUser([FromBody] ReqCreateUser req)
        {
            return await _userHandler.HandleAsync(req);
        }

        [HttpPost("SignInUser")]
        public async Task<IActionResult> SignInUser([FromBody] ReqSignInUser req)
        {
            var rsp = await _userHandler.HandleAsync(req);
            // 设置 HttpOnly Cookie
            CookieOptions options = new CookieOptions
            {
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(14) // 设置过期时间，例如14天
            };

            Response.Cookies.Append("refreshToken", rsp.RefreshToken, options);
            return Ok(rsp);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] ReqUpdateUser req)
        {
            await _userHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] ReqDeleteUser req)
        {
            await _userHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetUserInfo")]
        public async Task<RspGetUserInfo> GetUserInfo([FromBody] ReqGetUserInfo req)
        {
            return await _userHandler.GetUserInfoAsync(req);
        }

        [HttpPost("GetUserList")]
        public async Task<List<RspGetUserList>> GetUserList([FromBody] ReqGetUserList req)
            => await _userHandler.GetUserListAsync(req);

        [HttpPost("ValidateAccessToken")]
        public IActionResult ValidateAccessToken([FromBody] ReqValidateAccessToken req)
        {
            // 从 Authorization Header 中获取 Access Token
            string authorizationHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                // 请求头中没有 Access Token，返回错误响应
                return Unauthorized(new { message = "Access Token 无效。" });
            }

            string accessToken = authorizationHeader.Substring("Bearer ".Length);

            try
            {
                // 验证 Access Token 的签名和有效性
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // 不允许任何时钟偏差
                };

                // 验证 Access Token，如果验证通过，会将 Token 的 Claims 存储在 ClaimsPrincipal 中
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);

                // 在这里可以获取 Token 中的身份信息和其他相关信息
                string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string username = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                // 获取其他自定义的 Claim

                // Access Token 有效
                return Ok();
            }
            catch (Exception)
            {
                // Access Token 无效，返回错误响应
                return Unauthorized(new { message = "Access Token 无效。" });
            }

        }

        [HttpPost("RefreshAccessToken")]
        public IActionResult RefreshAccessToken([FromBody] ReqRefreshAccessToken req)
        {
            // TODO I Can't get any cookie here
            // # https://www.youtube.com/watch?v=ROg1p0UZL0M&ab_channel=IsraelQuiroz
            var refreshToken = HttpContext.Request.Cookies["refreshToken"];
            //string a = HttpContext.Request.Cookies["cookieName"] ;
            // string refreshToken = Request.Cookies["refreshToken"];

            return Ok();
        }
    }
}