using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;

namespace OrderSystemPlusTest.BusinessActor
{
    public class ProductManageHandlerTest
    {
        private IProductManageHandler _handler;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IProductInventoryManageHandler> _productInventoryHandler;
        private readonly Mock<IProductTypeRelationshipRepository> _productTypeRelationshipRepository;

        public ProductManageHandlerTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _productInventoryHandler = new Mock<IProductInventoryManageHandler>();
            _handler = new ProductManageHandler(
              _productInventoryHandler.Object,
              _productRepository.Object,
              _productTypeRelationshipRepository.Object);
        }

        [Fact]
        public async Task ProductCreate()
        {
            _productRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductDto>>()))
                .ReturnsAsync(new List<int> { 1 });
            _productInventoryHandler.Setup(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>())).ReturnsAsync(true);

            await _handler.HandleAsync(new List<ReqCreateProduct>
            {
                new ReqCreateProduct
                {
                    Name = "productName",
                    Description = "test",
                    Number = "TEST",
                    Quantity = 50,
                }
            });
            _productInventoryHandler.Verify(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>()), Times.Once());
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

        public async Task GetProductInfoAsync()
        {
            _productRepository
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<ProductDto> {
                new ProductDto{
                    Name = "Test",
                    Description = "Test",
                    Number = "TEST",
                }});

            var rsp = await _handler.GetProductInfoAsync(new ReqGetProductInfo { });
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
