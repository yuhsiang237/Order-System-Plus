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

        public ProductInventoryController(
            IProductInventoryQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
        }

        [HttpPost("GetProductInventoryList")]
        public async Task<List<RspGetProductInventoryList>> GetProductInventoryList([FromBody] ReqGetProductInventoryList req)
           => await _queryHandler.GetProductInventoryListAsync(req);

    }
}