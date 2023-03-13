using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Xunit;
using FluentAssertions;

using OrderSystemPlus.DataAccessor.Queries;
using OrderSystemPlus.DataAccessor;
using OrderSystemPlus.Models.DataAccessor.Commands;
using OrderSystemPlus.DataAccessor.Commands;
using OrderSystemPlus.Utils.GUIDSerialTool;

namespace OrderSystemPlusTest.DataAccessor
{
    public class ProductInventoryDataAccessorTest
    {
        private IProductInventoryQuery _query;
        private IInsertCommand<IEnumerable<ProductInventoryCommandModel>> _insert;
        private IDeleteCommand<IEnumerable<ProductInventoryCommandModel>> _delete;
        private IUpdateCommand<IEnumerable<ProductInventoryCommandModel>> _update;
        private DateTime _now;
        private string _guid;

        public ProductInventoryDataAccessorTest()
        {
            _now = DateTime.Now;
            _guid = GUIDSerialTool.Generate(15);
            _query = new ProductInventoryQuery();
            _insert = new ProductInventoryCommand();
            _delete = new ProductInventoryCommand();
            _update = new ProductInventoryCommand();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _insert.InsertAsync(GetInsertModel());
            var insertResult = await _query.FindByOptionsAsync(null, GetInsertModel().First().ProductId);
            insertResult.Count.Should().Be(1);
            insertResult.First().Description.Should().Be(GetInsertModel().First().Description);
            insertResult.First().ProductId.Should().Be(GetInsertModel().First().ProductId);
            insertResult.First().Quantity.Should().Be(GetInsertModel().First().Quantity);

            await _update.UpdateAsync(GetUpdateModel(insertResult.First().Id));
            var updateResult = await _query.FindByOptionsAsync(insertResult.First().Id, GetInsertModel().First().ProductId);
            updateResult.Count.Should().Be(1);
            updateResult.First().Description.Should().Be(GetUpdateModel(insertResult.First().Id).First().Description);
            updateResult.First().ProductId.Should().Be(GetUpdateModel(insertResult.First().Id).First().ProductId);
            updateResult.First().Quantity.Should().Be(GetUpdateModel(insertResult.First().Id).First().Quantity);


            await _delete.DeleteAsync(GetDeleteModel(updateResult.First().Id));
            var deleteResult = await _query.FindByOptionsAsync(updateResult.First().Id, null);
            deleteResult.Count.Should().Be(0);
        }

        public List<ProductInventoryCommandModel> GetInsertModel() => new List<ProductInventoryCommandModel> {
                new ProductInventoryCommandModel
                {
                    ProductId = 1,
                    Description = $"Test{_guid}",
                    Quantity = 100,
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                }
            };

        public List<ProductInventoryCommandModel> GetUpdateModel(int id) => new List<ProductInventoryCommandModel> {
                new ProductInventoryCommandModel
                {
                    Id = id,
                    ProductId = 1,
                    Description = $"TestUpdate{_guid}",
                    Quantity = 100,
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                }
            };

        public List<ProductInventoryCommandModel> GetDeleteModel(int id) => new List<ProductInventoryCommandModel> {
                 new ProductInventoryCommandModel
                {
                    Id = id,
                    UpdatedOn = _now,
                }
            };
    }
}
