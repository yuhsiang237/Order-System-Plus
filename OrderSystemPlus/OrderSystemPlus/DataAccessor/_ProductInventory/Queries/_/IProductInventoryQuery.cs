using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public interface IProductInventoryQuery
    {
        /// <summary>
        /// 查詢ProductInventory資料們
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<List<ProductInventoryQueryModel>> FindByOptionsAsync(int? id = null ,int? productId = null);
    }
}
