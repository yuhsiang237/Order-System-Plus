using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public interface IProductInventoryRepository
    {
        /// <summary>
        /// 刪除ProductInventory資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<ProductInventoryDto> model);

        /// <summary>
        /// 新增ProductInventory資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<List<int>> InsertAsync(IEnumerable<ProductInventoryDto> model);


        /// <summary>
        /// 查詢ProductInventory資料們
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<List<ProductInventoryDto>> FindByOptionsAsync(int? productId = null);
    }
}
