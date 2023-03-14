using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Moq;

using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.BusinessActor.Commands;
using OrderSystemPlus.Models.BusinessActor.Commands;
using OrderSystemPlus.Models.DataAccessor.Queries;

namespace OrderSystemPlusTest.BusinessActor.Commands
{
    public class ProductInventoryCommandHandlerTest
    {
        private ProductInventoryCommandHandler _handler;
        private readonly Mock<IProductQuery> _productQuery;
        private readonly Mock<IInsertCommand<IEnumerable<ProductInventoryCommandModel>>> _productInventoryInsertMock;

        public ProductInventoryCommandHandlerTest()
        {
            _productInventoryInsertMock = new Mock<IInsertCommand<IEnumerable<ProductInventoryCommandModel>>>();
            _productQuery = new Mock<IProductQuery>();

            _handler = new ProductInventoryCommandHandler(
                    _productInventoryInsertMock.Object,
                    _productQuery.Object);
        }

        [Fact]
        public async Task ProductInventoryCreate()
        {
            _productQuery
                .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<ProductQueryModel> { });

            _productInventoryInsertMock.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductInventoryCommandModel>>()));

            await _handler.HandleAsync(new ReqCreateProductInventory
            {
                ProductId = 999999,
                Quantity = 50,
            });
            _productQuery.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once);
            _productInventoryInsertMock.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ProductInventoryCommandModel>>()), Times.Once());
        }
    }
}
