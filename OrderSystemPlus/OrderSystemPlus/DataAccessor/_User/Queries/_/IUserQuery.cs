using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public interface IUserQuery
    {
        /// <summary>
        /// 查詢User資料們
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="email">信箱</param>
        /// <param name="account">帳號</param>
        /// <returns>List<UserQueryModel></returns>
        public Task<List<UserQueryModel>> FindByOptionsAsync(int? id, string? email, string? account);
    }
}
