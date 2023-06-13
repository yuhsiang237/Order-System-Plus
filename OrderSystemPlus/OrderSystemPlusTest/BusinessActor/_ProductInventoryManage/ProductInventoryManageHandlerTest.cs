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
    public class ProductInventoryManageHandlerTest
    {
        private IProductInventoryManageHandler _handler;
        private readonly Mock<IProductInventoryRepository> _productInventoryRepository;

        public ProductInventoryManageHandlerTest()
        {
            _productInventoryRepository = new Mock<IProductInventoryRepository>();
            _handler = new ProductInventoryManageHandler(
              _productInventoryRepository.Object);
        }

        [Fact]
        public async Task UpdateProductInventory()
        {
            _productInventoryRepository.Setup(x => x.FindByOptionsAsync();

            await _handler.HandleAsync(new List<ReqUpdateProductInventory>
            {
                new ReqUpdateProductInventory
                {
                }
            });
            _productInventoryHandler.Verify(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>()), Times.Once());
            _productInventoryRepository.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ProductDto>>()), Times.Once());
        }

        [Fact]
        public async Task GetProductListAsync()
        {
            _productInventoryRepository
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<ProductDto> {
                new ProductDto{
                    Name = "Test",
                    Description = "Test",
                    Number = "TEST",
                }});

            var rsp = await _handler.GetProductListAsync(new ReqGetProductList { });
            _productInventoryRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        public async Task GetProductInfoAsync()
        {
            _productInventoryRepository
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<ProductDto> {
                new ProductDto{
                    Name = "Test",
                    Description = "Test",
                    Number = "TEST",
                }});

            var rsp = await _handler.GetProductInfoAsync(new ReqGetProductInfo { });
            _productInventoryRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task ProductUpdate()
        {
            _productInventoryRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductDto>>()))
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
            _productInventoryRepository.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<ProductDto>>()), Times.Once());
        }

        [Fact]
        public async Task ProductDelete()
        {
            _productInventoryRepository.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductDto>>()));
            await _handler.HandleAsync(new List<ReqDeleteProduct>
            {
                new ReqDeleteProduct
                {
                    Id = 1,
                }
            });
            _productInventoryRepository.Verify(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductDto>>()), Times.Once());
        }
    }
}
