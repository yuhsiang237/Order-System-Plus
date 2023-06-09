using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlusTest.BusinessActor._ProductManage
{
    public class ProductManageHandlerTest
    {
        private IProductManageHandler _handler;
        private readonly Mock<IProductRepository> _productRepository;

        public ProductManageHandlerTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _handler = new ProductManageHandler(
              _productRepository.Object);
        }

        [Fact]
        public async Task ProductCreate()
        {
            _productRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductDto>>()))
                .ReturnsAsync(new List<int>());

            await _handler.HandleAsync(new List<ReqCreateProduct>
            {
                new ReqCreateProduct
                {
                    Name = "productName",
                    Description = "test",
                    Number = "TEST",
                }
            });
            _productRepository.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ProductDto>>()), Times.Once());
        }

        [Fact]
        public async Task GetProductListAsync()
        {
            _productRepository
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<ProductDto> {
                new ProductDto{
                    Name = "Test",
                    Description = "Test",
                    Number = "TEST",
                }});

            var rsp = await _handler.GetProductListAsync(new ReqGetProductList { });
            _productRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task ProductUpdate()
        {
            _productRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductDto>>()))
            .ReturnsAsync(new List<int>());

            await _handler.HandleAsync(new List<ReqUpdateProduct>
            {
                new ReqUpdateProduct
                {
                    Id = 1,
                    Name = "productName",
                    Description = "test",
                    Number = "TEST",
                }
            });
            _productRepository.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<ProductDto>>()), Times.Once());
        }

        [Fact]
        public async Task ProductDelete()
        {
            _productRepository.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductDto>>()));
            await _handler.HandleAsync(new List<ReqDeleteProduct>
            {
                new ReqDeleteProduct
                {
                    Id = 1,
                }
            });
            _productRepository.Verify(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductDto>>()), Times.Once());
        }
    }
}
