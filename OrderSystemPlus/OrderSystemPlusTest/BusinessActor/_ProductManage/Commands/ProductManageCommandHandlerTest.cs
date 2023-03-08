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
    public class ProductManageCommandHandlerTest
    {
        private ProductManageCommandHandler _handler;
        private readonly Mock<IProductTypeQuery> _query;
        private readonly Mock<IInsertCommand<IEnumerable<ProductTypeCommandModel>>> _insertMock;
        private readonly Mock<IUpdateCommand<IEnumerable<ProductTypeCommandModel>>> _updateMock;
        private readonly Mock<IDeleteCommand<IEnumerable<ProductTypeCommandModel>>> _deleteMock;

        public ProductManageCommandHandlerTest()
        {
            _insertMock = new Mock<IInsertCommand<IEnumerable<ProductTypeCommandModel>>>();
            _updateMock = new Mock<IUpdateCommand<IEnumerable<ProductTypeCommandModel>>>();
            _deleteMock = new Mock<IDeleteCommand<IEnumerable<ProductTypeCommandModel>>>();
            _query = new Mock<IProductTypeQuery>();
        }

        [Fact]
        public async Task ProductManageCreate()
        {
            _handler = new ProductManageCommandHandler(
                _insertMock.Object,
                _updateMock.Object,
                _deleteMock.Object,
                _query.Object);

            _query
                .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<ProductTypeQueryModel> { });

            _insertMock.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()));

            await _handler.HandleAsync(new ReqProductTypeCreate
            {
                Name = "productName",
                Description = "test"
            });
            _query.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()), Times.Once);
            _insertMock.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()), Times.Once());
        }

        [Fact]
        public async Task UserUpdate()
        {
            _handler = new ProductManageCommandHandler(
              _insertMock.Object,
              _updateMock.Object,
              _deleteMock.Object,
              _query.Object);


            _updateMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()));

            await _handler.HandleAsync(new ReqProductTypeUpdate
            {
                Id = 1,
                Name = "updateProductName",
            });
            _updateMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()), Times.Once());
        }

        [Fact]
        public async Task ProductManageDelete()
        {
            _handler = new ProductManageCommandHandler(
                _insertMock.Object,
                _updateMock.Object,
                _deleteMock.Object,
                _query.Object);
            _deleteMock.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()));
            await _handler.HandleAsync(new ReqProductTypeDelete
            {
                Id = 1,
            }); ;
            _deleteMock.Verify(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()), Times.Once());
        }
    }
}
