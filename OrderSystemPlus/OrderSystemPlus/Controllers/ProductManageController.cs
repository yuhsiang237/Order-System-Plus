using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductManageController : ControllerBase
    {
        private readonly IProductManageHandler _productHandler;

        public ProductManageController(
            IProductManageHandler productHandler)
        {
            _productHandler = productHandler;
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] List<ReqCreateProduct> req)
        {
            await _productHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromBody] List<ReqDeleteProduct> req)
        {
            await _productHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] List<ReqUpdateProduct> req)
        {
            await _productHandler.HandleAsync(req);
            return StatusCode(200);
        }


        [HttpPost("AdjustProductInventory")]
        public async Task<IActionResult> AdjustProductInventory([FromBody] List<ReqAdjustProductInventory> req)
        {
            await _productHandler.HandleAsync(req);
            return StatusCode(200);
        }
        [HttpPost("GetProductList")]
        public async Task<List<RspGetProductList>> GetProductList([FromBody] ReqGetProductList req)
            => await _productHandler.GetProductListAsync(req);
        
        [HttpPost("GetProductInfo")]
        public async Task<RspGetProductInfo> GetProductInfo([FromBody] ReqGetProductInfo req)
           => await _productHandler.GetProductInfoAsync(req);
    }
}