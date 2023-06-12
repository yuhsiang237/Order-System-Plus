using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.BusinessActor
{
    public class ProductTypeManageHandler : IProductTypeManageHandler
    {
        private readonly IProductTypeRepository _ProductTypeRepository;
        public ProductTypeManageHandler(
            IProductTypeRepository ProductTypeRepository)
        {
            _ProductTypeRepository = ProductTypeRepository;
        }

        public async Task<List<RspGetProductTypeList>> GetProductTypeListAsync(ReqGetProductTypeList req)
        {
            var data = await _ProductTypeRepository.FindByOptionsAsync(null, null);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductTypeDto, RspGetProductTypeList>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<List<ProductTypeDto>, List<RspGetProductTypeList>>(data);
            return rsp.ToList();
        }

        public async Task HandleAsync(List<ReqCreateProductType> req)
        {
            var now = DateTime.Now;
            var dtoList = req.Select(s => new ProductTypeDto
            {
                Name = s.Name,
                Description = s.Description,
                CreatedOn = now,
                UpdatedOn = now,
                IsValid = true,
            }).ToList();
            await _ProductTypeRepository.InsertAsync(dtoList);
        }

        public async Task HandleAsync(List<ReqUpdateProductType> req)
        {
            var now = DateTime.Now;
            var dtoList = req.Select(s => new ProductTypeDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                UpdatedOn = now,
            }).ToList();
            await _ProductTypeRepository.UpdateAsync(dtoList);
        }

        public async Task HandleAsync(List<ReqDeleteProductType> req)
        {
            var now = DateTime.Now;
            var dtoList = req.Select(s => new ProductTypeDto
            {
                Id = s.Id,
                UpdatedOn = now,
            }).ToList();
            await _ProductTypeRepository.DeleteAsync(dtoList);
        }
    }
}
