using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor.Commands;
using OrderSystemPlus.BusinessActor.Queries;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductInventoryController : ControllerBase
    {
        private readonly IProductInventoryQueryHandler _queryHandler;
        private readonly ProductInventoryCommandHandler _commandHandler;

        public ProductInventoryController(
            IProductInventoryQueryHandler queryHandler,
            ProductInventoryCommandHandler commandHandler)
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        [HttpPost("GetProductInventoryList")]
        public async Task<List<RspGetProductInventoryList>> GetProductInventoryList([FromBody] ReqGetProductInventoryList req)
           => await _queryHandler.GetProductInventoryListAsync(req);

        [HttpPost("ProductInventoryCreate")]
        public async Task<IActionResult> ProductInventoryCreate([FromBody] ReqProductInventoryCreate req)
        {
            await _commandHandler.HandleAsync(req);
            return StatusCode(200);
        }
    }
}