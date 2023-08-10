using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IAuthHandler
    {
        Task<(string AccessToken, string RefreshToken)> HandleAsync(ReqSignIn req);
        Task<RspRefreshAccessToken> HandleAsync(ReqRefreshAccessToken req);
        Task<bool> HandleAsync(ReqValidateAccessToken req);
    }
}
