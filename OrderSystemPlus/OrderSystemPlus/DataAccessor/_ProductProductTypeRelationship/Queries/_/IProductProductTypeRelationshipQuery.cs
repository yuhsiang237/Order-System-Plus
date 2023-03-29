using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public interface IProductProductTypeRelationshipQuery
    {
        /// <summary>
        /// 查詢ProductProductTypeRelationship資料們
        /// </summary>
        /// <param name="productIds">產品productIds</param>
        /// <param name="productTypeIds">產品類別productTypeIds</param>
        /// <returns></returns>
        public Task<List<ProductProductTypeRelationshipQueryModel>> FindByOptionsAsync(
            List<int>? productIds = null,
            List<int>? productTypeIds = null);
    }
}
