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
        private readonly Mock<IProductTypeQuery> _productTypeQuery;
        private readonly Mock<IProductQuery> _productQuery;
        private readonly Mock<IInsertCommand<IEnumerable<ProductTypeCommandModel>>> _productTypeInsertMock;
        private readonly Mock<IUpdateCommand<IEnumerable<ProductTypeCommandModel>>> _productTypeUpdateMock;
        private readonly Mock<IDeleteCommand<IEnumerable<ProductTypeCommandModel>>> _productTypeDeleteMock;
        private readonly Mock<IInsertCommand<IEnumerable<ProductCommandModel>>> _productInsertMock;
        private readonly Mock<IUpdateCommand<IEnumerable<ProductCommandModel>>> _productUpdateMock;
        private readonly Mock<IDeleteCommand<IEnumerable<ProductCommandModel>>> _productDeleteMock;

        public ProductManageCommandHandlerTest()
        {
            _productTypeInsertMock = new Mock<IInsertCommand<IEnumerable<ProductTypeCommandModel>>>();
            _productTypeUpdateMock = new Mock<IUpdateCommand<IEnumerable<ProductTypeCommandModel>>>();
            _productTypeDeleteMock = new Mock<IDeleteCommand<IEnumerable<ProductTypeCommandModel>>>();
            _productTypeQuery = new Mock<IProductTypeQuery>();

            _productInsertMock = new Mock<IInsertCommand<IEnumerable<ProductCommandModel>>>();
            _productUpdateMock = new Mock<IUpdateCommand<IEnumerable<ProductCommandModel>>>();
            _productDeleteMock = new Mock<IDeleteCommand<IEnumerable<ProductCommandModel>>>();
            _productQuery = new Mock<IProductQuery>();
            
            _handler = new ProductManageCommandHandler(
                    _productTypeInsertMock.Object,
                    _productTypeUpdateMock.Object,
                    _productTypeDeleteMock.Object,
                    _productInsertMock.Object,
                    _productUpdateMock.Object,
                    _productDeleteMock.Object,
                    _productTypeQuery.Object,
                    _productQuery.Object);
        }

        [Fact]
        public async Task ProductTypeCreate()
        {
            _productTypeQuery
                .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<ProductTypeQueryModel> { });

            _productTypeInsertMock.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()));

            await _handler.HandleAsync(new ReqProductTypeCreate
            {
                Name = "productName",
                Description = "test"
            });
            _productTypeQuery.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()), Times.Once);
            _productTypeInsertMock.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()), Times.Once());
        }

        [Fact]
        public async Task ProductTypeUpdate()
        {
            _productTypeUpdateMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()));

            await _handler.HandleAsync(new ReqProductTypeUpdate
            {
                Id = 1,
                Name = "updateProductName",
            });
            _productTypeUpdateMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()), Times.Once());
        }

        [Fact]
        public async Task ProductTypeDelete()
        {
            _productTypeDeleteMock.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()));
            await _handler.HandleAsync(new ReqProductTypeDelete
            {
                Id = 1,
            }); ;
            _productTypeDeleteMock.Verify(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductTypeCommandModel>>()), Times.Once());
        }

        [Fact]
        public async Task ProductCreate()
        {
            _productQuery
                .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()))
                .ReturnsAsync(new List<ProductQueryModel> { });

            _productInsertMock.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<ProductCommandModel>>()));

            await _handler.HandleAsync(new ReqProductCreate
            {
                Name = "productName",
                Description = "test",
                Number = "TEST",
            });
            _productQuery.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once);
            _productInsertMock.Verify(x => x.InsertAsync(It.IsAny<IEnumerable<ProductCommandModel>>()), Times.Once());
        }

        [Fact]
        public async Task ProductDelete()
        {
            _productDeleteMock.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductCommandModel>>()));
            await _handler.HandleAsync(new ReqProductDelete
            {
                Id = 1,
            }); ;
            _productDeleteMock.Verify(x => x.DeleteAsync(It.IsAny<IEnumerable<ProductCommandModel>>()), Times.Once());
        }

        [Fact]
        public async Task ProductUpdate()
        {
            _productUpdateMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<ProductCommandModel>>()));

            await _handler.HandleAsync(new ReqProductUpdate
            {
                Id = 1,
                Name = "productName",
                Description = "test",
                Number = "TEST",
            });
            _productUpdateMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<ProductCommandModel>>()), Times.Once());
        }
    }
}
