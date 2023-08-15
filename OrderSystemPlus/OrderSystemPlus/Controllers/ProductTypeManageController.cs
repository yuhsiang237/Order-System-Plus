using Microsoft.AspNetCore.Mvc;

using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlus.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductTypeManageController : ControllerBase
    {
        private readonly IProductTypeManageHandler _ProductTypeHandler;

        public ProductTypeManageController(
            IProductTypeManageHandler ProductTypeHandler)
        {
            _ProductTypeHandler = ProductTypeHandler;
        }

        [HttpPost("CreateProductType")]
        public async Task<IActionResult> CreateProductType([FromBody] List<ReqCreateProductType> req)
        {
            await _ProductTypeHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("DeleteProductType")]
        public async Task<IActionResult> DeleteProductType([FromBody] List<ReqDeleteProductType> req)
        {
            await _ProductTypeHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("UpdateProductType")]
        public async Task<IActionResult> UpdateProductType([FromBody] List<ReqUpdateProductType> req)
        {
            await _ProductTypeHandler.HandleAsync(req);
            return StatusCode(200);
        }

        [HttpPost("GetProductTypeList")]
        public async Task<RspGetProductTypeList> GetProductTypeList([FromBody] ReqGetProductTypeList req)
            => await _ProductTypeHandler.GetProductTypeListAsync(req);
    }
}