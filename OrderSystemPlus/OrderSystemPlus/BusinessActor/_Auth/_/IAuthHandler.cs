using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IAuthHandler
    {
        Task<(string AccessToken, string RefreshToken)> HandleAsync(ReqSignIn req);
        Task HandleAsync(ReqRefreshAccessToken req);
        Task HandleAsync(ReqValidateAccessToken req);
    }
}
