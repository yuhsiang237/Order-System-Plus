﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlusTest.BusinessActor
{
    public class ProductTypeManageHandlerTest
    {
        private IProductTypeManageHandler _handler;
        private readonly Mock<IProductTypeRepository> _productRepository;

        public ProductTypeManageHandlerTest()
        {
            _productRepository = new Mock<IProductTypeRepository>();
            _handler = new ProductTypeManageHandler(
              _productRepository.Object);
        }

        [Fact]
        public async Task ProductTypeCreate()
        {
            _productRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductTypeDto>>()))
                .ReturnsAsync(new List<int>());

            await _handler.HandleAsync(new List<ReqCreateProductType>
            {
                new ReqCreateProductType
                {
                    Name = "productName",
                    Description = "test",
                }
            });
            _productRepository.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ProductTypeDto>>()), Times.Once());
        }

        [Fact]
        public async Task GetProductTypeListAsync()
        {
            _productRepository
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<ProductTypeDto> {
                new ProductTypeDto{
                    Name = "Test",
                    Description = "Test",
                }});

            var rsp = await _handler.GetProductTypeListAsync(new ReqGetProductTypeList { });
            _productRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task ProductTypeUpdate()
        {
            _productRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductTypeDto>>()))
            .ReturnsAsync(new List<int>());

            await _handler.HandleAsync(new List<ReqUpdateProductType>
            {
                new ReqUpdateProductType
                {
                    Id = 1,
                    Name = "productName",
                    Description = "test",
                }
            });
            _productRepository.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<ProductTypeDto>>()), Times.Once());
        }

        [Fact]
        public async Task ProductTypeDelete()
        {
            _productRepository.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductTypeDto>>()));
            await _handler.HandleAsync(new List<ReqDeleteProductType>
            {
                new ReqDeleteProductType
                {
                    Id = 1,
                }
            });
            _productRepository.Verify(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductTypeDto>>()), Times.Once());
        }
    }
}