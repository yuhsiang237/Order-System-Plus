using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.BusinessActor.Queries
{
    public class ProductManageQueryHandler : IProductManageQueryHandler
    {
        private readonly IProductTypeQuery _productTypeQuery;
        private readonly IProductQuery _productQuery;

        public ProductManageQueryHandler(
            IProductTypeQuery productTypeQuery,
            IProductQuery productQuery)
        {
            _productTypeQuery = productTypeQuery;
            _productQuery = productQuery;
        }

        public async Task<List<RspGetProductTypeList>> GetProductTypeListAsync(ReqGetProductTypeList req)
        {
            var data = await _productTypeQuery.FindByOptionsAsync(null, null);
            return data.Select(x => new RspGetProductTypeList
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToList();
        }

        public async Task<List<RspGetProductList>> GetProductListAsync(ReqGetProductList req)
        {
            var data = await _productQuery.FindByOptionsAsync(null, null, null);
            return data.Select(x => new RspGetProductList
            {
                Id = x.Id,
                Name = x.Name,
                Number = x.Number,
                CurrentUnit = x.CurrentUnit,
                Price = x.Price,
                Description = x.Description,
            }).ToList();
        }
    }
}
