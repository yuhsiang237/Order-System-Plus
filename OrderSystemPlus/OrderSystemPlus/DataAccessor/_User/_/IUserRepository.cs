using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public interface IUserRepository
    {
        /// <summary>
        /// 更新User資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<UserDto> model);

        /// <summary>
        /// 刪除User資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<UserDto> model);

        /// <summary>
        /// 新增User資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> InsertAsync(UserDto model);


        /// <summary>
        /// 查詢User資料們
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<List<UserDto>> FindByOptionsAsync(int? id = null, string? email = null, string? account = null);
    }
}
