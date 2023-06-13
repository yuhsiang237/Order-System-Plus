using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;
using OrderSystemPlus.BusinessActor;
using OrderSystemPlus.Models.BusinessActor;
using OrderSystemPlus.Enums;

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
        public async Task GetProductInventoryHistoryListAsync()
        {
            _productInventoryRepository.Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()))
               .ReturnsAsync(new List<ProductInventoryDto>
            {
                new ProductInventoryDto
                {
                    ProductId = 99999,
                    AdjustQuantity = 50,
                    Remark = "5",
                    PrevTotalQuantity = 0,
                    TotalQuantity = 50,
                    AdjustProductInventoryType = AdjustProductInventoryType.Force,
                }
            });
            await _handler.GetProductInventoryHistoryListAsync(new ReqGetProductInventoryHistoryList
            {
            });
            _productInventoryRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()), Times.Once());
        }

        [Fact]
        public async Task GetProductCurrentTotalQuantityAsync()
        {
            _productInventoryRepository.Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()))
               .ReturnsAsync(new List<ProductInventoryDto>
            {
                new ProductInventoryDto
                {
                    ProductId = 99999,
                    AdjustQuantity = 50,
                    Remark = "5",
                    PrevTotalQuantity = 0,
                    TotalQuantity = 50,
                    AdjustProductInventoryType = AdjustProductInventoryType.Force,
                }
            });
            await _handler.GetProductCurrentTotalQuantityAsync(new ReqGetProductCurrentTotalQuantity
            {
            });
            _productInventoryRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()), Times.Once());
        }

        [Fact]
        public async Task UpdateProductInventory()
        {
            _productInventoryRepository.Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new List<ProductInventoryDto>
            {
                new ProductInventoryDto
                {
                    ProductId = 99999,
                    AdjustQuantity = 50,
                    Remark = "5",
                    AdjustProductInventoryType = AdjustProductInventoryType.Force,
                }
            });
            _productInventoryRepository.Setup(x => x.InsertAsync(It.IsAny<List<ProductInventoryDto>>()))
                .ReturnsAsync(new List<int> { 1 });

            await _handler.HandleAsync(new List<ReqUpdateProductInventory>
            {
                new ReqUpdateProductInventory
                {
                    ProductId = 99999,
                    AdjustQuantity = 50,
                    Description = "5",
                    Type = AdjustProductInventoryType.Force,
                }
            });
            _productInventoryRepository.Verify(x => x.InsertAsync(It.IsAny<List<ProductInventoryDto>>()), Times.Once());
            _productInventoryRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()), Times.Once());
        }
    }
}
