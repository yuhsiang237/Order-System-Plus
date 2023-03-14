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
    public class ProductInventoryQueryHandlerTest
    {
        private IProductInventoryQueryHandler _handler;
        private readonly Mock<IProductInventoryQuery> _productInventoryQuery;

        public ProductInventoryQueryHandlerTest()
        {
            _productInventoryQuery = new Mock<IProductInventoryQuery>();
        }

        [Fact]
        public async Task GetProductInventoryListAsync()
        {
            _productInventoryQuery
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync(new List<ProductInventoryQueryModel> {
                new ProductInventoryQueryModel{
                    ProductId = 1,
                    Description = "Test",
                    Quantity = 100,
                }});
            _handler = new ProductInventoryQueryHandler(
               _productInventoryQuery.Object);
            var rsp = await _handler.GetProductInventoryListAsync(
            new ReqGetProductInventoryList
            {

            });
            _productInventoryQuery.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<int?>()), Times.Once());
        }
    }
}
