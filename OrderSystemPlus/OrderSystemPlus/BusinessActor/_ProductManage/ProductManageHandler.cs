using System.Transactions;
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

        private readonly IProductInventoryManageHandler _productInventoryManageHandler;

        public ProductManageHandler(
            IProductInventoryManageHandler productInventoryControlHandler,
            IProductRepository productRepository,
            IProductTypeRelationshipRepository productTypeRelationshipRepository)
        {
            _productInventoryManageHandler = productInventoryControlHandler;
            _productRepository = productRepository;
            _productTypeRelationshipRepository = productTypeRelationshipRepository;
        }

        public async Task<RspGetProductList> GetProductListAsync(ReqGetProductList req)
        {
            var data = await _productRepository.FindByOptionsAsync(
                likeName: req.Name,
                likeNumber: req.Number, 
                pageIndex: req.PageIndex,
                pageSize: req.PageSize,
                sortField: req.SortField,
                sortType: req.SortType
                );
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDto, RspGetProductListItem>()
                   .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                   .ForMember(dest => dest.ProductTypeIds, opt => opt.Ignore());
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<List<ProductDto>, List<RspGetProductListItem>>(data.Data);

            foreach (var item in rsp)
            {
                item.Quantity = await _productInventoryManageHandler.GetProductCurrentTotalQuantityAsync(new ReqGetProductCurrentTotalQuantity
                {
                    ProductId = item.Id,
                });

                item.ProductTypeIds = (await _productTypeRelationshipRepository.FindByOptionsAsync(productId: item.Id))
                    ?.Select(s => s?.ProductTypeId)
                    ?.ToList();
            }

            return new RspGetProductList
            {
                Data = rsp,
                TotalCount = data.TotalCount,
            };
        }

        public async Task<RspGetProductInfo> GetProductInfoAsync(ReqGetProductInfo req)
        {
            var data = (await _productRepository.FindByOptionsAsync(id: req.Id)).Data.FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDto, RspGetProductInfo>()
                   .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                   .ForMember(dest => dest.ProductTypeIds, opt => opt.Ignore());
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<ProductDto, RspGetProductInfo>(data);
            rsp.Quantity = await _productInventoryManageHandler.GetProductCurrentTotalQuantityAsync(new ReqGetProductCurrentTotalQuantity
            {
                ProductId = req.Id,
            });

            rsp.ProductTypeIds = (await _productTypeRelationshipRepository.FindByOptionsAsync(productId: req.Id))
                ?.Select(s => s?.ProductTypeId)
                ?.ToList();
            return rsp;
        }

        public async Task HandleAsync(ReqCreateProduct req)
        {
            var isExist = (await _productRepository.FindByOptionsAsync(number: req.Number)).Data.Any();
            if (isExist)
                throw new BusinessException("已存在相同產品編號");

            var now = DateTime.Now;
            var dtoList = new List<ProductDto>
            {
            new ProductDto
                        {
                            Name = req.Name,
                            Number = req.Number,
                            Price = req.Price.Value,
                            Description = req.Description,
                            CreatedOn = now,
                            UpdatedOn = now,
                            IsValid = true,
                        }
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var productIds = await _productRepository.InsertAsync(dtoList);
            var productTypeRelationshipDtoList = new List<ProductTypeRelationshipDto>();
            for (var i = 0; i < productIds.Count; i++)
            {
                var ProductTypeIds =
                 req.ProductTypeIds?.Select(productTypeId =>
                     new ProductTypeRelationshipDto
                     {
                         ProductId = productIds[i],
                         ProductTypeId = productTypeId,
                     }).ToList() ?? new List<ProductTypeRelationshipDto>();

                productTypeRelationshipDtoList.AddRange(ProductTypeIds);
            }
            var productTypeRelationshipResult = await _productTypeRelationshipRepository.RefreshAsync(productTypeRelationshipDtoList);
            if (productTypeRelationshipResult == false)
                throw new Exception("productTypeRelationshipResult Error");

            var inventoryDtoList = new List<ReqUpdateProductInventory>();
            for (var i = 0; i < productIds.Count; i++)
            {
                inventoryDtoList.Add(new ReqUpdateProductInventory
                {
                    ProductId = productIds[i],
                    Type = AdjustProductInventoryType.Force,
                    AdjustQuantity = req.Quantity,
                    Description = "商品建立庫存。"
                });
            }
            var inventoryResult = await _productInventoryManageHandler.HandleAsync(inventoryDtoList);
            if (inventoryResult == false)
                throw new Exception("AdjustProductInventoryAsync Error");

            scope.Complete();
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

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var productTypeRelationshipDtoList = new List<ProductTypeRelationshipDto>();
            for (var i = 0; i < dtoList.Count; i++)
            {
                var ProductTypeIds =
                req[i].ProductTypeIds?.Select(productTypeId =>
                    new ProductTypeRelationshipDto
                    {
                        ProductId = dtoList[i].Id,
                        ProductTypeId = productTypeId,
                    }).ToList() ?? new List<ProductTypeRelationshipDto>();

                productTypeRelationshipDtoList.AddRange(ProductTypeIds);
            }
            await _productRepository.UpdateAsync(dtoList);
            var productTypeRelationshipResult = await _productTypeRelationshipRepository.RefreshAsync(productTypeRelationshipDtoList);
            if (productTypeRelationshipResult == false)
                throw new Exception("productTypeRelationshipResult Error");
            scope.Complete();
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
