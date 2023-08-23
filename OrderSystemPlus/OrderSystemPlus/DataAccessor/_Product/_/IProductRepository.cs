using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public interface IProductRepository
    {
        /// <summary>
        /// 更新Product資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<ProductDto> model);

        /// <summary>
        /// 刪除Product資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<ProductDto> model);

        /// <summary>
        /// 新增Product資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<List<int>> InsertAsync(IEnumerable<ProductDto> model);


        /// <summary>
        /// 查詢Product資料們
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<(int TotalCount, List<ProductDto> Data)> FindByOptionsAsync(int? id = null, string? name = null, string? number = null, string? likeName = null, string? likeNumber = null, 
            int? pageIndex = null,
            int? pageSize = null,
            string? sortField = null,
            SortType? sortType = null);
    }
}
