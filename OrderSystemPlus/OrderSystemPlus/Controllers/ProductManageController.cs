using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor.Commands;
using OrderSystemPlus.BusinessActor.Queries;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductManageController : ControllerBase
    {
        private readonly ProductManageCommandHandler _commandHandler;
        private readonly IProductManageQueryHandler _queryHandler;

        public ProductManageController(
            ProductManageCommandHandler commandHandler,
            IProductManageQueryHandler queryHandler)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
        }

        [HttpPost("CreateProductType")]
        public async Task<IActionResult> CreateProductType([FromBody] ReqCreateProductType req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("DeleteProductType")]
        public async Task<IActionResult> DeleteProductType([FromBody] ReqDeleteProductType req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("UpdateProductType")]
        public async Task<IActionResult> UpdateProductType([FromBody] ReqUpdateProductType req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetProductTypeList")]
        public async Task<List<RspGetProductTypeList>> GetProductTypeList([FromBody] ReqGetProductTypeList req)
            => await _queryHandler.GetProductTypeListAsync(req);

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ReqCreateProduct req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromBody] ReqDeleteProduct req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ReqUpdateProduct req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetProductList")]
        public async Task<List<RspGetProductList>> GetProductList([FromBody] ReqGetProductList req)
            => await _queryHandler.GetProductListAsync(req);

    }
}