using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace OrderSystemPlus.Utils.JwtHelper
{
    public interface IJwtHelper
    {
        /// <summary>
        /// Generate token
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GenerateToken(string account);
    }
}