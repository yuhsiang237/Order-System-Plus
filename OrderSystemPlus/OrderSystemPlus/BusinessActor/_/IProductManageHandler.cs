using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IProductManageHandler
    {
        Task HandleAsync(List<ReqCreateProduct> req);
        Task HandleAsync(List<ReqUpdateProduct> req);
        Task HandleAsync(List<ReqDeleteProduct> req);
        Task<List<RspGetProductList>> GetProductListAsync(ReqGetProductList req);
    }
}
