using System.Linq;

using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.BusinessActor
{
    public class ProductManageHandler : IProductManageHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRelationshipRepository _productTypeRelationshipRepository;

        private readonly IProductInventoryManageHandler _productInventoryControlHandler;

        public ProductManageHandler(
            IProductInventoryManageHandler productInventoryControlHandler,
            IProductRepository productRepository,
            IProductTypeRelationshipRepository productTypeRelationshipRepository)
        {
            _productInventoryControlHandler = productInventoryControlHandler;
            _productRepository = productRepository;
            _productTypeRelationshipRepository = productTypeRelationshipRepository;
        }

        public async Task<List<RspGetProductList>> GetProductListAsync(ReqGetProductList req)
        {
            var data = await _productRepository.FindByOptionsAsync(null, null, null);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDto, RspGetProductList>()
                   .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                   .ForMember(dest => dest.ProductTypeIds, opt => opt.Ignore());
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<List<ProductDto>, List<RspGetProductList>>(data);

            foreach (var item in rsp)
            {
                item.Quantity = await _productInventoryControlHandler.GetProductCurrentTotalQuantityAsync(new ReqGetProductCurrentTotalQuantity
                {
                    ProductId = item.Id,
                });

                item.ProductTypeIds = (await _productTypeRelationshipRepository.FindByOptionsAsync(productId: item.Id))
                    ?.Select(s => s?.ProductTypeId)
                    ?.ToList();
            }

            return rsp.ToList();
        }

        public async Task<RspGetProductInfo> GetProductInfoAsync(ReqGetProductInfo req)
        {
            var data = (await _productRepository.FindByOptionsAsync(id: req.Id)).FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDto, RspGetProductInfo>()
                   .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                   .ForMember(dest => dest.ProductTypeIds, opt => opt.Ignore());
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<ProductDto, RspGetProductInfo>(data);
            rsp.Quantity = await _productInventoryControlHandler.GetProductCurrentTotalQuantityAsync(new ReqGetProductCurrentTotalQuantity
            {
                ProductId = req.Id,
            });

            rsp.ProductTypeIds = (await _productTypeRelationshipRepository.FindByOptionsAsync(productId: req.Id))
                ?.Select(s => s?.ProductTypeId)
                ?.ToList();
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

            var productIds = await _productRepository.InsertAsync(dtoList);
            var productTypeRelationshipDtoList = new List<ProductTypeRelationshipDto>();
            for (var i = 0; i < productIds.Count; i++)
            {
                productTypeRelationshipDtoList.AddRange(
                req[i].ProductTypeIds.Select(productTypeId =>
                    new ProductTypeRelationshipDto
                    {
                        ProductId = productIds[i],
                        ProductTypeId = productTypeId,
                    }).ToList()
                );
            }
            await _productTypeRelationshipRepository.RefreshAsync(productTypeRelationshipDtoList);
            var inventoryDtoList = new List<ReqUpdateProductInventory>();
            for (var i = 0; i < productIds.Count; i++)
            {
                inventoryDtoList.Add(new ReqUpdateProductInventory
                {
                    ProductId = productIds[i],
                    Type = AdjustProductInventoryType.Force,
                    AdjustQuantity = req[i].Quantity,
                    Description = "商品建立庫存。"
                });
            }
            var inventoryResult = await _productInventoryControlHandler.HandleAsync(inventoryDtoList);
            if (inventoryResult == false)
                throw new Exception("AdjustProductInventoryAsync Error");
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

            var productTypeRelationshipDtoList = new List<ProductTypeRelationshipDto>();
            for (var i = 0; i < dtoList.Count; i++)
            {
                productTypeRelationshipDtoList.AddRange(
                req[i].ProductTypeIds.Select(productTypeId =>
                    new ProductTypeRelationshipDto
                    {
                        ProductId = dtoList[i].Id,
                        ProductTypeId = productTypeId,
                    }).ToList()
                );
            }
            await _productRepository.UpdateAsync(dtoList);
            await _productTypeRelationshipRepository.RefreshAsync(productTypeRelationshipDtoList);
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
