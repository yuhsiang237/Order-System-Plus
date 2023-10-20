using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IShipmentOrderManageHandler
    {
        Task<string> HandleAsync(ReqCreateShipmentOrder req);
        Task HandleAsync(ReqUpdateShipmentOrder req);
        Task HandleAsync(ReqDeleteShipmentOrder req);
        Task<RspGetShipmentOrderInfo> GetShipmentOrderInfoAsync(ReqGetShipmentOrderInfo req);
        Task<RspGetShipmentOrderList> GetShipmentOrderListAsync(ReqGetShipmentOrderList req);
    }
}
