using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IProductInventoryManageHandler
    {
        Task<List<RspGetProductInventoryHistoryList>> GetProductInventoryHistoryListAsync(ReqGetProductInventoryHistoryList req);
        Task<decimal?> GetProductCurrentTotalQuantityAsync(ReqGetProductCurrentTotalQuantity req);
        Task<bool> HandleAsync(List<ReqUpdateProductInventory> req);
    }
}
