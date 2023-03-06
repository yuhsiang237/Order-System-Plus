using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.BusinessActor.Queries;

public interface IUserManageQueryHandler
{
    /// <summary>
    /// GetUserListAsync
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<List<RspGetUserList>> GetUserListAsync(ReqGetUserList req);

    /// <summary>
    /// GetUserInfoAsync
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<RspGetUserInfo> GetUserInfoAsync(ReqGetUserInfo req);
}
