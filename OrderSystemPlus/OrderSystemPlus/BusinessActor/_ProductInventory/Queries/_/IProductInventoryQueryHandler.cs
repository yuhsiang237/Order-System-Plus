using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.BusinessActor.Queries;

public interface IProductInventoryQueryHandler
{
    /// <summary>
    /// GetProductInventoryListAsync
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<List<RspGetProductInventoryList>> GetProductInventoryListAsync(ReqGetProductInventoryList req);
}
