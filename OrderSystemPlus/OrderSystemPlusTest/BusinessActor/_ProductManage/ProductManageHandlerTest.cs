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
    public class ProductManageHandlerTest
    {
        private IProductManageHandler _handler;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IProductTypeRepository> _productTypeRepository;
        private readonly Mock<IProductInventoryManageHandler> _productInventoryHandler;
        private readonly Mock<IProductTypeRelationshipRepository> _productTypeRelationshipRepository;

        public ProductManageHandlerTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _productTypeRelationshipRepository = new Mock<IProductTypeRelationshipRepository>();
            _productTypeRepository = new Mock<IProductTypeRepository>();

            _productInventoryHandler = new Mock<IProductInventoryManageHandler>();
            _handler = new ProductManageHandler(
              _productInventoryHandler.Object,
              _productRepository.Object,
              _productTypeRepository.Object,
              _productTypeRelationshipRepository.Object);
        }

        [Fact]
        public async Task ProductCreate()
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
                }));
            _productRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductDto>>()))
                .ReturnsAsync(new List<int> { 1 });
            _productInventoryHandler.Setup(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>())).ReturnsAsync(true);
            _productTypeRelationshipRepository.Setup(x => x.RefreshAsync(It.IsAny<IEnumerable<ProductTypeRelationshipDto>>())).ReturnsAsync(true);


            await _handler.HandleAsync(new ReqCreateProduct
            {
                Name = "productName",
                Description = "test",
                Number = "TEST",
                Quantity = 50,
                Price = 50,
                ProductTypeIds = new List<int?> { 1 }
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
            _productTypeRelationshipRepository.Verify(x => x.RefreshAsync(It.IsAny<IEnumerable<ProductTypeRelationshipDto>>()), Times.Once());
            _productInventoryHandler.Verify(x => x.HandleAsync(It.IsAny<List<ReqUpdateProductInventory>>()), Times.Once());
            _productRepository.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ProductDto>>()), Times.Once());
        }

        [Fact]
        public async Task GetProductListAsync()
        {
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

            var rsp = await _handler.GetProductListAsync(new ReqGetProductList { });

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
        }

        public async Task GetProductInfoAsync()
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
                    Id = 1,
                    Name = "Test",
                    Description = "Test",
                    Number = "TEST",
                }}));
            var rsp = await _handler.GetProductInfoAsync(new ReqGetProductInfo { });
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
        }

        [Fact]
        public async Task ProductUpdate()
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
                   Id = 1,
                        Name = "Test",
                        Description = "Test",
                        Number = "TEST",
                        Price = 500,
                }}));
            _productRepository.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductDto>>()))
            .ReturnsAsync(new List<int>());
            _productTypeRelationshipRepository.Setup(x => x.RefreshAsync(It.IsAny<IEnumerable<ProductTypeRelationshipDto>>())).ReturnsAsync(true);

            await _handler.HandleAsync(
                new ReqUpdateProduct
                {
                    Id = 1,
                    Name = "productName",
                    Description = "test",
                    Number = "TEST",
               
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
            _productTypeRelationshipRepository.Verify(x => x.RefreshAsync(It.IsAny<IEnumerable<ProductTypeRelationshipDto>>()), Times.Once());
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
