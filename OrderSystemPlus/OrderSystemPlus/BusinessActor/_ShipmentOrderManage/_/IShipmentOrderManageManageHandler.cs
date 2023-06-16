using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.BusinessActor
{
    public interface IShipmentOrderManageManageHandler
    {
        Task HandleAsync(ReqCreateShipmentOrder req);
        Task HandleAsync(ReqUpdateShipmentOrder req);
        Task HandleAsync(ReqDeleteShipmentOrder req);
        Task<RspGetShipmentOrderInfo> GetShipmentOrderInfoAsync(ReqGetShipmentOrderInfo req);
    }
}
