using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.BusinessActor
{
    public class ProductManageHandler : IProductManageHandler
    {
        private readonly IProductRepository _productRepository;
        public ProductManageHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<RspGetProductList>> GetProductListAsync(ReqGetProductList req)
        {
            var data = await _productRepository.FindByOptionsAsync(null, null, null);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDto, RspGetProductList>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<List<ProductDto>, List<RspGetProductList>>(data);
            return rsp.ToList();
        }

        public async Task<RspGetProductInfo> GetProductInfoAsync(ReqGetProductInfo req)
        {
            var data = (await _productRepository.FindByOptionsAsync(null, null, null)).FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDto, RspGetProductInfo>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<ProductDto, RspGetProductInfo>(data);
            return rsp;
        }

        public async Task HandleAsync(List<ReqCreateProduct> req)
        {
            var now = DateTime.Now;
            var dtoList = req.Select(s => new ProductDto
            {
                Name = s.Name,
                Number = s.Number,
                Price = s.Price,
                Description = s.Description,
                CreatedOn = now,
                UpdatedOn = now,
                IsValid = true,
            }).ToList();
            await _productRepository.InsertAsync(dtoList);
        }

        public async Task HandleAsync(List<ReqUpdateProduct> req)
        {
            var now = DateTime.Now;
            var dtoList = req.Select(s => new ProductDto
            {
                Id = s.Id,
                Name = s.Name,
                Number = s.Number,
                Price = s.Price,
                Description = s.Description,
                UpdatedOn = now,
            }).ToList();
            await _productRepository.UpdateAsync(dtoList);
        }

        public async Task HandleAsync(List<ReqDeleteProduct> req)
        {
            var now = DateTime.Now;
            var dtoList = req.Select(s => new ProductDto
            {
                Id = s.Id,
                UpdatedOn = now,
            }).ToList();
            await _productRepository.DeleteAsync(dtoList);
        }
    }
}
