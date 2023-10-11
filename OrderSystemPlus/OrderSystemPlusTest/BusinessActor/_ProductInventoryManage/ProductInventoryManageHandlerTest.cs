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
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IProductTypeRepository> _productTypeRepository;
        private readonly Mock<IProductTypeRelationshipRepository> _productTypeRelationshipRepository;

        public ProductInventoryManageHandlerTest()
        {
            _productInventoryRepository = new Mock<IProductInventoryRepository>();
            _productRepository = new Mock<IProductRepository>();
            _productTypeRelationshipRepository = new Mock<IProductTypeRelationshipRepository>();
            _productTypeRepository = new Mock<IProductTypeRepository>();

            _handler = new ProductInventoryManageHandler(
              _productInventoryRepository.Object,
              _productRepository.Object,
              _productTypeRepository.Object,
              _productTypeRelationshipRepository.Object);
        }
        [Fact]
        public async Task GetProductInventoryListAsync()
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
            _productTypeRepository
           .Setup(x => x.FindByOptionsAsync(
               It.IsAny<int?>(),
               It.IsAny<string?>(),
               It.IsAny<string?>(),
               It.IsAny<int?>(),
               It.IsAny<int?>(),
               It.IsAny<string?>(),
               It.IsAny<SortType?>()
           ))
           .ReturnsAsync((0,
           new List<ProductTypeDto> { }));
            _productRepository
            .Setup(x => x.FindByOptionsAsync(
            It.IsAny<int?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<string?>(),
            It.IsAny<SortType?>()))
            .ReturnsAsync((1, new List<ProductDto> {
                new ProductDto{
                    Id = 1,
                    Name = "Test",
                    Description = "Test",
                    Number = "TEST",
                }}));

            _productTypeRelationshipRepository.Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync(new List<ProductTypeRelationshipDto>
            {
                new ProductTypeRelationshipDto
                {
                    ProductId = 1,
                    ProductTypeId = 1
                }
            });

            var rsp = await _handler.GetProductInventoryListAsync(new ReqGetProductInventoryList { });

            _productTypeRelationshipRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()), Times.Once());
            _productRepository.Verify(x => x.FindByOptionsAsync(
            It.IsAny<int?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<string?>(),
            It.IsAny<SortType?>()), Times.Once());

            _productTypeRepository
           .Verify(x => x.FindByOptionsAsync(
               It.IsAny<int?>(),
               It.IsAny<string?>(),
               It.IsAny<string?>(),
               It.IsAny<int?>(),
               It.IsAny<int?>(),
               It.IsAny<string?>(),
               It.IsAny<SortType?>()
           ), Times.Once());
            _productInventoryRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()), Times.Once());
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
            _productRepository
           .Setup(x => x.FindByOptionsAsync(
           It.IsAny<int?>(),
           It.IsAny<string?>(),
           It.IsAny<string?>(),
           It.IsAny<string?>(),
           It.IsAny<string?>(),
           It.IsAny<int?>(),
           It.IsAny<int?>(),
           It.IsAny<string?>(),
           It.IsAny<SortType?>()))
           .ReturnsAsync((1, new List<ProductDto> {
                new ProductDto{
                       Id = 99999,
                    Name = "Test",
                    Description = "Test",
                    Number = "TEST",
                }}));

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
            _productRepository.Verify(x => x.FindByOptionsAsync(
                      It.IsAny<int?>(),
                      It.IsAny<string?>(),
                      It.IsAny<string?>(),
                      It.IsAny<string?>(),
                      It.IsAny<string?>(),
                      It.IsAny<int?>(),
                      It.IsAny<int?>(),
                      It.IsAny<string?>(),
                      It.IsAny<SortType?>()), Times.Once()); 
            _productInventoryRepository.Verify(x => x.InsertAsync(It.IsAny<List<ProductInventoryDto>>()), Times.Once());
            _productInventoryRepository.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()), Times.Once());
        }
    }
}
