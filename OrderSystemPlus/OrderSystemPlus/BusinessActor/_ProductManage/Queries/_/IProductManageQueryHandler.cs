using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.BusinessActor.Queries;

public interface IProductManageQueryHandler
{
    /// <summary>
    /// GetProductTypeListAsync
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<List<RspGetProductTypeList>> GetProductTypeListAsync(ReqGetProductTypeList req);
}
