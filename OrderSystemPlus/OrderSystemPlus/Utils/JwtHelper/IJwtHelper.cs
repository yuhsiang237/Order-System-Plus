namespace OrderSystemPlus.Utils.JwtHelper
{
    public interface IJwtHelper
    {
        public string GenerateRefreshToken(string userId, string username, int expiresInDays = 14);
        public string GenerateAccessToken(string userId, string username, int expiresInMinutes);
    }
}