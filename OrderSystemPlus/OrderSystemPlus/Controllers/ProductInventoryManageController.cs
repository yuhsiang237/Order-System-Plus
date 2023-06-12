using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductInventoryManageController : ControllerBase
    {
        private readonly IProductInventoryManageHandler _productInventoryHandler;

        public ProductInventoryManageController(
            IProductInventoryManageHandler productInventoryHandler)
        {
            _productInventoryHandler = productInventoryHandler;
        }

        [HttpPost("UpdateProductInventory")]
        public async Task<IActionResult> UpdateProductInventory([FromBody] List<ReqUpdateProductInventory> req)
        {
            await _productInventoryHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetProductInventoryHistoryList")]
        public async Task<List<RspGetProductInventoryHistoryList>> GetProductInventoryHistoryList([FromBody] ReqGetProductInventoryHistoryList req)
        {
            return await _productInventoryHandler.GetProductInventoryHistoryListAsync(req);
        }

        [HttpPost("GetProductInventoryInfo")]
        public async Task<decimal?> GetProductInventoryInfo([FromBody] ReqGetProductInventoryInfo req)
        {
            return await _productInventoryHandler.GetProductInventoryInfoAsync(req.ProductId);
        }
    }
}