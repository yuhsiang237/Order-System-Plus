using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlus.DataAccessor.Queries
{
    public interface IProductQuery
    {
        /// <summary>
        /// 查詢Product資料們
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<List<ProductQueryModel>> FindByOptionsAsync(int? id,string? name, string? number);
    }
}
