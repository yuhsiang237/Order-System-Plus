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
        private readonly Mock<IProductTypeQuery> _query;

        public ProductManageQueryHandlerTest()
        {
            _query = new Mock<IProductTypeQuery>();
        }

        [Fact]
        public async Task GetProductTypeListAsync()
        {
            _query
            .Setup(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()))
            .ReturnsAsync(new List<ProductTypeQueryModel> {
                new ProductTypeQueryModel{
                    Name = "Test",
                    Description = "Test",
                }});
            _handler = new ProductManageQueryHandler(
               _query.Object);
            var rsp = await _handler.GetProductTypeListAsync(
            new ReqGetProductTypeList
            {

            });
            _query.Verify(x => x.FindByOptionsAsync(It.IsAny<int?>(), It.IsAny<string?>()), Times.Once());
        }
    }
}
