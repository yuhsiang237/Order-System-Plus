using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReturnShipmentOrderManageController : ControllerBase
    {
        private readonly IReturnShipmentOrderManageHandler _returnShipmentOrderHandler;

        public ReturnShipmentOrderManageController(
            IReturnShipmentOrderManageHandler returnShipmentOrderHandler)
        {
            _returnShipmentOrderHandler = returnShipmentOrderHandler;
        }

        [HttpPost("CreateReturnShipmentOrder")]
        public async Task<string> CreateReturnShipmentOrder([FromBody] ReqCreateReturnShipmentOrder req)
        {
            var rsp = await _returnShipmentOrderHandler.HandleAsync(req);
            return rsp;
        }

        //[HttpPost("UpdateReturnShipmentOrder")]
        //public async Task<IActionResult> UpdateReturnShipmentOrder([FromBody] ReqUpdateReturnShipmentOrder req)
        //{
        //    await _returnShipmentOrderHandler.HandleAsync(req);
        //    return StatusCode(200);
        //}

        [HttpPost("GetReturnShipmentOrderList")]
        public async Task<List<RspGetReturnShipmentOrderList>> GetReturnShipmentOrderList([FromBody] ReqGetReturnShipmentOrderList req)
        {
            var rsp = await _returnShipmentOrderHandler.GetReturnShipmentOrderListAsync(req);
            return rsp;
        }

        [HttpPost("GetReturnShipmentOrderInfo")]
        public async Task<RspGetReturnShipmentOrderInfo> GetReturnShipmentOrderInfo([FromBody] ReqGetReturnShipmentOrderInfo req)
        {
            var rsp = await _returnShipmentOrderHandler.GetReturnShipmentOrderInfoAsync(req);
            return rsp;
        }

        [HttpPost("DeleteReturnShipmentOrder")]
        public async Task<IActionResult> DeleteReturnShipmentOrder([FromBody] ReqDeleteReturnShipmentOrder req)
        {
            await _returnShipmentOrderHandler.HandleAsync(req);
            return StatusCode(200);
        }
    }
}