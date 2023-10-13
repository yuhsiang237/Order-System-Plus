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

        [HttpPost("GetProductInventoryList")]
        public async Task<RspGetProductInventoryList> GetProductList([FromBody] ReqGetProductInventoryList req)
            => await _productInventoryHandler.GetProductInventoryListAsync(req);

        [HttpPost("GetProductInventoryHistoryList")]
        public async Task<List<RspGetProductInventoryHistoryList>> GetProductInventoryHistoryList([FromBody] ReqGetProductInventoryHistoryList req)
        {
            return await _productInventoryHandler.GetProductInventoryHistoryListAsync(req);
        }

        [HttpPost("GetProductCurrentTotalQuantity")]
        public async Task<decimal?> GetProductCurrentTotalQuantityAsync([FromBody] ReqGetProductCurrentTotalQuantity req)
        {
            return await _productInventoryHandler.GetProductCurrentTotalQuantityAsync(new ReqGetProductCurrentTotalQuantity
            {
                ProductId = req.ProductId,
            });
        }
    }
}