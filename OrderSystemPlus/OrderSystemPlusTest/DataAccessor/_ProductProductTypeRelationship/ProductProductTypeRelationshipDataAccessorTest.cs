using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Xunit;
using FluentAssertions;

using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.DataAccessor.Commands;

namespace OrderSystemPlusTest.DataAccessor
{
    public class ProductProductTypeRelationshipDataAccessorTest
    {
        private IProductProductTypeRelationshipQuery _query;
        private IInsertCommand<IEnumerable<ProductProductTypeRelationshipCommandModel>> _insert;
        private IDeleteCommand<IEnumerable<ProductProductTypeRelationshipCommandModel>> _delete;

        public ProductProductTypeRelationshipDataAccessorTest()
        {
            _query = new ProductProductTypeRelationshipQuery();
            _insert = new ProductProductTypeRelationshipCommand();
            _delete = new ProductProductTypeRelationshipCommand();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _insert.InsertAsync(GetInsertModel());
            var insertResult = await _query.FindByOptionsAsync(GetInsertModel().Select(s => s.ProductId).ToList(), null);
            insertResult.Count.Should().Be(1);
            insertResult.First().ProductId.Should().Be(GetInsertModel().First().ProductId);
            insertResult.First().ProductTypeId.Should().Be(GetInsertModel().First().ProductTypeId);

            await _delete.DeleteAsync(GetDeleteModel(insertResult.First().Id));
            var deleteResult = await _query.FindByOptionsAsync(new List<int> { insertResult.First().ProductId }, null);
            deleteResult.Count.Should().Be(0);
        }

        public List<ProductProductTypeRelationshipCommandModel> GetInsertModel() => new List<ProductProductTypeRelationshipCommandModel> {
                new ProductProductTypeRelationshipCommandModel
                {
                   ProductId = 999999,
                   ProductTypeId = 999999,
                },
            };
        public List<ProductProductTypeRelationshipCommandModel> GetDeleteModel(int id) => new List<ProductProductTypeRelationshipCommandModel> {
                 new ProductProductTypeRelationshipCommandModel
                {
                    Id = id,
                }
            };
    }
}
