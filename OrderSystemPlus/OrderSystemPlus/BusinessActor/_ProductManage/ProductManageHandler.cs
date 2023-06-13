﻿using AutoMapper;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Enums;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlus.BusinessActor
{
    public class ProductManageHandler : IProductManageHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductInventoryManageHandler _productInventoryControlHandler;

        public ProductManageHandler(
            IProductInventoryManageHandler productInventoryControlHandler,
            IProductRepository productRepository)
        {
            _productInventoryControlHandler = productInventoryControlHandler;
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

            foreach (var item in rsp)
            {
                item.Quantity = await _productInventoryControlHandler.GetProductCurrentTotalQuantityAsync(new ReqGetProductCurrentTotalQuantity
                {
                    ProductId = item.Id,
                });
            }

            return rsp.ToList();
        }

        public async Task<RspGetProductInfo> GetProductInfoAsync(ReqGetProductInfo req)
        {
            var data = (await _productRepository.FindByOptionsAsync(id: req.Id)).FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDto, RspGetProductInfo>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var rsp = mapper.Map<ProductDto, RspGetProductInfo>(data);
            rsp.Quantity = await _productInventoryControlHandler.GetProductCurrentTotalQuantityAsync(new ReqGetProductCurrentTotalQuantity
            {
                ProductId = req.Id,
            });
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
            var productInsertResult = await _productRepository.InsertAsync(dtoList);

            var inventoryDtoList = new List<ReqUpdateProductInventory>();
            for (var i = 0; i < productInsertResult.Count; i++)
            {
                inventoryDtoList.Add(new ReqUpdateProductInventory
                {
                    ProductId = productInsertResult[i],
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
