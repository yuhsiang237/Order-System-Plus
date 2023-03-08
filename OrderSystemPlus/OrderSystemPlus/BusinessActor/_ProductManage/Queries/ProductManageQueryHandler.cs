using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.BusinessActor.Queries
{
    public class ProductManageQueryHandler : IProductManageQueryHandler
    {
        private readonly IProductTypeQuery _query;

        public ProductManageQueryHandler(IProductTypeQuery query)
        {
            _query = query;
        }

        public async Task<List<RspGetProductTypeList>> GetProductTypeListAsync(ReqGetProductTypeList req)
        {
            var data = await _query.FindByOptionsAsync(null, null);
            return data.Select(x => new RspGetProductTypeList
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToList();
        }
    }
}
