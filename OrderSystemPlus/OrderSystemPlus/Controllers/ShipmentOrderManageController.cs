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
    }
}