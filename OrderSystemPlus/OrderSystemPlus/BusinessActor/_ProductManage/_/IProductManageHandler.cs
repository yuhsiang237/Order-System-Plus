using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IProductManageHandler
    {
        Task HandleAsync(List<ReqCreateProduct> req);
        Task HandleAsync(List<ReqUpdateProduct> req);
        Task HandleAsync(List<ReqDeleteProduct> req);
        Task<RspGetProductInfo> GetProductInfoAsync(ReqGetProductInfo req);
        Task<RspGetProductList> GetProductListAsync(ReqGetProductList req);
    }
}
