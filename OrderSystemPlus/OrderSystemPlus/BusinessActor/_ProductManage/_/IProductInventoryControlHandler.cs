using OrderSystemPlus.Models.BusinessActor;
namespace OrderSystemPlus.BusinessActor
{
    public interface IProductInventoryControlHandler
    {
        Task<decimal?> GetProductInventoryAsync(int productId);
        Task<bool> AdjustProductInventoryAsync(List<ReqAdjustProductInventory> req);
    }
}
