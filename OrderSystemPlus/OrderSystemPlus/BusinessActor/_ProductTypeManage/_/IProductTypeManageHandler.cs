using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IProductTypeManageHandler
    {
        Task HandleAsync(List<ReqCreateProductType> req);
        Task HandleAsync(List<ReqUpdateProductType> req);
        Task HandleAsync(List<ReqDeleteProductType> req);
        Task<List<RspGetProductTypeList>> GetProductTypeListAsync(ReqGetProductTypeList req);
    }
}
