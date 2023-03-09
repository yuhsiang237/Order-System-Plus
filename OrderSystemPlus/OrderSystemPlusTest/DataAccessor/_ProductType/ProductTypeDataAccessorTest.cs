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
    public class ProductTypeDataAccessorTest
    {
        private IProductTypeQuery _query;
        private IInsertCommand<IEnumerable<ProductTypeCommandModel>> _insert;
        private IDeleteCommand<IEnumerable<ProductTypeCommandModel>> _delete;
        private IUpdateCommand<IEnumerable<ProductTypeCommandModel>> _update;
        private DateTime _now;
        private string _guid;

        public ProductTypeDataAccessorTest()
        {
            _now = DateTime.Now;
            _guid = GUIDSerialTool.Generate(15);
            _query = new ProductTypeQuery();
            _insert = new ProductTypeCommand();
            _delete = new ProductTypeCommand();
            _update = new ProductTypeCommand();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _insert.InsertAsync(GetTypeInsertModel());
            var insertResult = await _query.FindByOptionsAsync(null, GetTypeInsertModel().First().Name);
            insertResult.Count.Should().Be(1);
            insertResult.First().Name.Should().Be(GetTypeInsertModel().First().Name);
            insertResult.First().Description.Should().Be(GetTypeInsertModel().First().Description);

            await _update.UpdateAsync(GetTypeUpdateModel(insertResult.First().Id));
            var updateResult = await _query.FindByOptionsAsync(insertResult.First().Id, null);
            updateResult.Count.Should().Be(1);
            updateResult.First().Name.Should().Be(GetTypeUpdateModel(updateResult.First().Id).First().Name);
            updateResult.First().Description.Should().Be(GetTypeUpdateModel(updateResult.First().Id).First().Description);


            await _delete.DeleteAsync(GetDeleteModel(updateResult.First().Id));
            var deleteResult = await _query.FindByOptionsAsync(updateResult.First().Id, null);
            deleteResult.Count.Should().Be(0);
        }

        public List<ProductTypeCommandModel> GetTypeInsertModel() => new List<ProductTypeCommandModel> {
                new ProductTypeCommandModel
                {
                    Name = $"Test{_guid}",
                    Description = $"Test{_guid}",
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                }
            };

        public List<ProductTypeCommandModel> GetTypeUpdateModel(int id) => new List<ProductTypeCommandModel> {
                new ProductTypeCommandModel
                {
                    Id = id,
                    Name = $"UpdateTest{_guid}",
                    Description = $"UpdateTest{_guid}",
                    IsValid = true,
                    UpdatedOn = _now,
                }
            };

        public List<ProductTypeCommandModel> GetDeleteModel(int id) => new List<ProductTypeCommandModel> {
                 new ProductTypeCommandModel
                {
                    Id = id,
                    UpdatedOn = _now,
                }
            };
    }
}
