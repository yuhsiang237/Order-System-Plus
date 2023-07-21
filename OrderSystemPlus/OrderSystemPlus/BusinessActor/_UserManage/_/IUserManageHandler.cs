using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IUserManageHandler
    {
        Task<RspSignInUser> HandleAsync(ReqSignInUser req);
        Task<int> HandleAsync(ReqCreateUser req);
        Task HandleAsync(ReqUpdateUser req);
        Task HandleAsync(ReqDeleteUser req);
        Task HandleAsync(ReqRefreshAccessToken req);
        Task HandleAsync(ReqValidateAccessToken req);

        Task<RspGetUserInfo> GetUserInfoAsync(ReqGetUserInfo req);
        Task<List<RspGetUserList>> GetUserListAsync(ReqGetUserList req);
    }
}
