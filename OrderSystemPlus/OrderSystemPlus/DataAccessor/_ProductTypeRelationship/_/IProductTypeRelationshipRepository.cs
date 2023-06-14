using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.DataAccessor
{
    public interface IProductTypeRelationshipRepository
    {
        /// <summary>
        /// 重建ProductTypeRelationship資料們
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<List<int>> RefreshAsync(IEnumerable<ProductTypeRelationshipDto> model);

        /// <summary>
        /// 查詢ProductTypeRelationship資料們
        /// </summary>
        /// <returns></returns>
        Task<List<ProductTypeRelationshipDto>> FindByOptionsAsync(int? productId = null, int? productTypeId = null);   }
}
