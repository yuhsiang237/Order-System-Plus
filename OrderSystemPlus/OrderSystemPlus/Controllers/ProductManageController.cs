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

        [HttpPost("ProductTypeCreate")]
        public async Task<IActionResult> ProductTypeCreate([FromBody] ReqProductTypeCreate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("ProductTypeDelete")]
        public async Task<IActionResult> ProductTypeDelete([FromBody] ReqProductTypeDelete req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("ProductTypeUpdate")]
        public async Task<IActionResult> ProductTypeUpdate([FromBody] ReqProductTypeUpdate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetProductTypeList")]
        public async Task<List<RspGetProductTypeList>> GetProductTypeList([FromBody] ReqGetProductTypeList req)
            => await _queryHandler.GetProductTypeListAsync(req);

        [HttpPost("ProductCreate")]
        public async Task<IActionResult> ProductCreate([FromBody] ReqProductCreate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("ProductDelete")]
        public async Task<IActionResult> ProductDelete([FromBody] ReqProductDelete req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("ProductUpdate")]
        public async Task<IActionResult> ProductUpdate([FromBody] ReqProductUpdate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetProductList")]
        public async Task<List<RspGetProductList>> GetProductList([FromBody] ReqGetProductList req)
            => await _queryHandler.GetProductListAsync(req);

    }
}