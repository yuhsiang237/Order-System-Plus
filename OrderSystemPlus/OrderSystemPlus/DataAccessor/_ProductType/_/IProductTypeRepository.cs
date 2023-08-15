using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public interface IProductTypeRepository
    {
        /// <summary>
        /// 更新ProductType資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<ProductTypeDto> model);

        /// <summary>
        /// 刪除ProductType資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<ProductTypeDto> model);

        /// <summary>
        /// 新增ProductType資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<List<int>> InsertAsync(IEnumerable<ProductTypeDto> model);


        /// <summary>
        /// 查詢ProductType資料們
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        Task<(int TotalCount, List<ProductTypeDto> Data)> FindByOptionsAsync(
            int? id = null, 
            string? name = null,
            int? pageIndex = null,
            int? pageSize = null,
            string? sortField = null,
            SortType? sortType = null);
    }
}
