using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.Models.DataAccessor.Queries;
using OrderSystemPlus.BusinessActor.Queries;
using OrderSystemPlus.Models.BusinessActor.Queries;

namespace OrderSystemPlusTest.BusinessActor.Queries
{
    public class ProductManageQueryHandlerTest
    {
        private IProductManageQueryHandler _handler;
        private readonly Mock<IProductTypeQuery> _productTypeQuery;
        private readonly Mock<IProductQuery> _productQuery;

        public ProductManageQueryHandlerTest()
        {
            _productTypeQuery = new Mock<IProductTypeQuery>();
            _productQuery = new Mock<IProductQuery>();
        }

        [Fact]
        public async Task GetProductTypeListAsync()
        {
            _productTypeQuery
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<ProductTypeQueryModel> {
                new ProductTypeQueryModel{
                    Name = "Test",
                    Description = "Test",
                }});
            _handler = new ProductManageQueryHandler(
               _productTypeQuery.Object,
               _productQuery.Object);
            var rsp = await _handler.GetProductTypeListAsync(
            new ReqGetProductTypeList
            {

            });
            _productTypeQuery.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()), Times.Once());
        }

        [Fact]
        public async Task GetProductListAsync()
        {
            _productQuery
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<ProductQueryModel> {
                new ProductQueryModel{
                    Name = "Test",
                    Description = "Test",
                    Number = "TEST",
                }});
            _handler = new ProductManageQueryHandler(
               _productTypeQuery.Object,
               _productQuery.Object);
            var rsp = await _handler.GetProductListAsync(
            new ReqGetProductList
            {

            });
            _productQuery.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once());
        }
    }
}
