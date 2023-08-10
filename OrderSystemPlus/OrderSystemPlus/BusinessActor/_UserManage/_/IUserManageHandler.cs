using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IUserManageHandler
    {
        Task<int> HandleAsync(ReqCreateUser req);
        Task HandleAsync(ReqUpdateUser req);
        Task HandleAsync(ReqDeleteUser req);
        Task<RspGetUserInfo> GetUserInfoAsync(ReqGetUserInfo req);
        Task<List<RspGetUserList>> GetUserListAsync(ReqGetUserList req);
    }
}
