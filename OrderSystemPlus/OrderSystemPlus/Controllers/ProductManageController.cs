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
        public async Task<IActionResult> CreateProductType([FromBody] ReqProductTypeCreate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("DeleteProductType")]
        public async Task<IActionResult> DeleteProductType([FromBody] ReqProductTypeDelete req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("UpdateProductType")]
        public async Task<IActionResult> UpdateProductType([FromBody] ReqProductTypeUpdate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetProductTypeList")]
        public async Task<List<RspGetProductTypeList>> GetProductTypeList([FromBody] ReqGetProductTypeList req)
            => await _queryHandler.GetProductTypeListAsync(req);

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ReqProductCreate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromBody] ReqProductDelete req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ReqProductUpdate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetProductList")]
        public async Task<List<RspGetProductList>> GetProductList([FromBody] ReqGetProductList req)
            => await _queryHandler.GetProductListAsync(req);

    }
}