using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public interface IProductTypeRelationshipRepository
    {
        /// <summary>
        /// 刪除ProductTypeRelationship資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task DeleteAsync(ProductTypeRelationshipDto model);

        /// <summary>
        /// 新增ProductTypeRelationship資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<List<int>> InsertAsync(IEnumerable<ProductTypeRelationshipDto> model);

        /// <summary>
        /// 查詢ProductTypeRelationship資料們
        /// </summary>
        /// <returns></returns>
        Task<List<ProductTypeRelationshipDto>> FindByOptionsAsync(int? productId = null, int? productTypeId = null);   }
}
