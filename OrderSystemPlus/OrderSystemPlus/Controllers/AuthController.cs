using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthHandler _authHandler;
        private IConfiguration _configuration;
        private readonly int _refreshTokenExpireDay;

        public AuthController(
            IAuthHandler authHandler,
            IConfiguration configuration)
        {
            _authHandler = authHandler;
            _configuration = configuration;
            _refreshTokenExpireDay = _configuration.GetValue<int>("JwtSettings:RefreshTokenExpireDay");
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] ReqSignIn req)
        {
            var rsp = await _authHandler.HandleAsync(req);
            // Set HttpOnly Cookie
            CookieOptions options = new CookieOptions
            {
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(_refreshTokenExpireDay)
            };

            Response.Cookies.Append("refreshToken", rsp.RefreshToken, options);
            return Ok(new RspSignIn
            {
                AccessToken = rsp.AccessToken,
            });
        }

        [HttpPost("SignOut")]
        public IActionResult SignOut()
        {
            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true,
            });

            return Ok();
        }

        [HttpPost("ValidateAccessToken")]
        public async Task<IActionResult> ValidateAccessToken()
        {
            string authorizationHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                return Unauthorized(new { message = "Access Token Error" });

            string accessToken = authorizationHeader.Substring("Bearer ".Length);
            var isValid = await _authHandler.HandleAsync(
                new ReqValidateAccessToken { AccessToken = accessToken });

            if (isValid)
                return Ok();
            else
                return Unauthorized(new { message = "Access Token Error" });
        }

        [HttpPost("RefreshAccessToken")]
        public async Task<RspRefreshAccessToken> RefreshAccessToken()
        {
            var refreshToken = HttpContext.Request.Cookies["refreshToken"];
            return await _authHandler
                .HandleAsync(
                new ReqRefreshAccessToken { RefreshToken = refreshToken });
        }
    }
}