using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.BusinessActor.Queries
{
    public class ProductInventoryQueryHandler : IProductInventoryQueryHandler
    {
        private readonly IProductInventoryQuery _productInventoryQuery;

        public ProductInventoryQueryHandler(
            IProductInventoryQuery productInventoryQuery)
        {
            _productInventoryQuery = productInventoryQuery;
        }

        public async Task<List<RspGetProductInventoryList>> GetProductInventoryListAsync(ReqGetProductInventoryList req)
        {
            var data = await _productInventoryQuery.FindByOptionsAsync(req.Id, req.ProductId);
            return data
                .Select(x => new RspGetProductInventoryList
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    ActionType = x.ActionType,
                }).ToList();
        }
    }
}
