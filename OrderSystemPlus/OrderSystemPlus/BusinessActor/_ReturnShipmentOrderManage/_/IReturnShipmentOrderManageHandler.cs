using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IReturnShipmentOrderManageHandler
    {
        Task<string> HandleAsync(ReqCreateReturnShipmentOrder req);
        Task HandleAsync(ReqUpdateReturnShipmentOrder req);
        Task HandleAsync(ReqDeleteReturnShipmentOrder req);
        Task<RspGetReturnShipmentOrderInfo> GetReturnShipmentOrderInfoAsync(ReqGetReturnShipmentOrderInfo req);
        Task<RspGetReturnShipmentOrderList> GetReturnShipmentOrderListAsync(ReqGetReturnShipmentOrderList req);
    }
}
