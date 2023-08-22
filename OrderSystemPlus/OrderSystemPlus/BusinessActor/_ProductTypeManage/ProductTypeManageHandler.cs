using System.Linq;
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

        public async Task<RspGetProductTypeList> GetProductTypeListAsync(ReqGetProductTypeList req)
        {
            var (totalCount, data) = await _ProductTypeRepository
                                            .FindByOptionsAsync(name: req.Name,
                                                                id: req.Id,
                                                                pageIndex: req.PageIndex,
                                                                pageSize: req.PageSize,
                                                                sortField: req.SortField,
                                                                sortType: req.SortType);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductTypeDto, RspGetProductTypeListItem>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<List<ProductTypeDto>, List<RspGetProductTypeListItem>>(data);

            return new RspGetProductTypeList
            {
                Data = rsp,
                TotalCount = totalCount,
            };
        }

        public async Task HandleAsync(ReqCreateProductType req)
        {
            var isExist = (await _ProductTypeRepository.FindByOptionsAsync(name: req.Name)).Data.Any();
            if (isExist)
                throw new BusinessException("已存在名稱");

            var now = DateTime.Now;
            await _ProductTypeRepository.InsertAsync(
                new List<ProductTypeDto>
                {
                    new ProductTypeDto
                    {
                        Name = req.Name,
                        Description = req.Description,
                        CreatedOn = now,
                        UpdatedOn = now,
                        IsValid = true,
                    }
                }
          );
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
