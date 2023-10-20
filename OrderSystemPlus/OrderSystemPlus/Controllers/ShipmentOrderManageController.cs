using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipmentOrderManageController : ControllerBase
    {
        private readonly IShipmentOrderManageHandler _shipmentOrderHandler;

        public ShipmentOrderManageController(
            IShipmentOrderManageHandler shipmentOrderHandler)
        {
            _shipmentOrderHandler = shipmentOrderHandler;
        }

        [HttpPost("CreateShipmentOrder")]
        public async Task<string> CreateShipmentOrder([FromBody] ReqCreateShipmentOrder req)
        {
            var rsp = await _shipmentOrderHandler.HandleAsync(req);
            return rsp;
        }

        [HttpPost("UpdateShipmentOrder")]
        public async Task<IActionResult> UpdateShipmentOrder([FromBody] ReqUpdateShipmentOrder req)
        {
            await _shipmentOrderHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetShipmentOrderList")]
        public async Task<RspGetShipmentOrderList> GetShipmentOrderList([FromBody] ReqGetShipmentOrderList req)
        {
            var rsp = await _shipmentOrderHandler.GetShipmentOrderListAsync(req);
            return rsp;
        }

        [HttpPost("GetShipmentOrderInfo")]
        public async Task<RspGetShipmentOrderInfo> GetShipmentOrderInfo([FromBody] ReqGetShipmentOrderInfo req)
        {
            var rsp = await _shipmentOrderHandler.GetShipmentOrderInfoAsync(req);
            return rsp;
        }

        [HttpPost("DeleteShipmentOrder")]
        public async Task<IActionResult> DeleteShipmentOrder([FromBody] ReqDeleteShipmentOrder req)
        {
            await _shipmentOrderHandler.HandleAsync(req);
            return StatusCode(200);
        }
    }
}