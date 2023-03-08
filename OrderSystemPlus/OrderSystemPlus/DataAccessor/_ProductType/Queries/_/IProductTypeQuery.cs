using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public interface IProductTypeQuery
    {
        /// <summary>
        /// 查詢ProductType資料們
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<List<ProductTypeQueryModel>> FindByOptionsAsync(int? id,string? name);
    }
}
