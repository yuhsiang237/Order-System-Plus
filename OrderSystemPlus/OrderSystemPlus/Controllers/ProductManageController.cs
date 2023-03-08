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

    }
}