using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Xunit;
using FluentAssertions;

using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor;

namespace OrderSystemPlusTest.DataAccessor
{
    public class ProductTypeRelationshipDataAccessorTest
    {
        private IProductTypeRelationshipRepository _repository;

        public ProductTypeRelationshipDataAccessorTest()
        {
            _repository = new ProductTypeRelationshipRepository();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _repository.RefreshAsync(new List<ProductTypeRelationshipDto> { GetRefreshModel() });
            var rsp = await _repository.FindByOptionsAsync(GetRefreshModel().ProductId);
            rsp.Count.Should().Be(1);
            rsp.First().ProductId.Should().Be(GetRefreshModel().ProductId);
            rsp.First().ProductTypeId.Should().Be(GetRefreshModel().ProductTypeId);
        }

        public ProductTypeRelationshipDto GetRefreshModel() =>
               new ProductTypeRelationshipDto
               {
                   ProductId = 999999999,
                   ProductTypeId = 888888888,
               };
    }
}
