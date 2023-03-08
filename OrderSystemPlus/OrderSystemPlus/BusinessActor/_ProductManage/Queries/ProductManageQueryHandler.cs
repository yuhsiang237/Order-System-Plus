using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.BusinessActor.Queries
{
    public class UserManageQueryHandler : IUserManageQueryHandler
    {
        private readonly IUserQuery _query;

        public UserManageQueryHandler(IUserQuery query)
        {
            _query = query;
        }

        public async Task<RspGetUserInfo> GetUserInfoAsync(ReqGetUserInfo req)
        {
            var rsp = (await _query.FindByOptionsAsync(
                req.Id,
                null,
                null))
                .FirstOrDefault();

            return new RspGetUserInfo
            {
                Id = rsp.Id,
                Name = rsp.Name,
                Account = rsp.Account,
                Email = rsp.Email,
            };
        }

        public async Task<List<RspGetUserList>> GetUserListAsync(ReqGetUserList req)
        {
            var rsp = await _query.FindByOptionsAsync(null, null, null);
            var result = rsp.Select(x =>
                new RspGetUserList
                {
                    Id = x.Id,
                    Name = x.Name,
                    Account = x.Account,
                    Email = x.Email,
                })
                .ToList();
            return result;
        }
    }
}
