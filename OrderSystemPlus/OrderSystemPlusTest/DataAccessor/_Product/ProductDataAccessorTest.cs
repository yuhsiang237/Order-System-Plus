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
    public class ProductDataAccessorTest
    {
        private IProductQuery _query;
        private IInsertCommand<IEnumerable<ProductCommandModel>> _insert;
        private IDeleteCommand<IEnumerable<ProductCommandModel>> _delete;
        private IUpdateCommand<IEnumerable<ProductCommandModel>> _update;
        private DateTime _now;
        private string _guid;

        public ProductDataAccessorTest()
        {
            _now = DateTime.Now;
            _guid = GUIDSerialTool.Generate(15);
            _query = new ProductQuery();
            _insert = new ProductCommand();
            _delete = new ProductCommand();
            _update = new ProductCommand();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _insert.InsertAsync(GetInsertModel());
            var insertResult = await _query.FindByOptionsAsync(null, GetInsertModel().First().Name, GetInsertModel().First().Number);
            insertResult.Count.Should().Be(1);
            insertResult.First().Name.Should().Be(GetInsertModel().First().Name);
            insertResult.First().Description.Should().Be(GetInsertModel().First().Description);
            insertResult.First().CurrentUnit.Should().Be(GetInsertModel().First().CurrentUnit);
            insertResult.First().Number.Should().Be(GetInsertModel().First().Number);
            insertResult.First().Price.Should().Be(GetInsertModel().First().Price);

            await _update.UpdateAsync(GetUpdateModel(insertResult.First().Id));
            var updateResult = await _query.FindByOptionsAsync(insertResult.First().Id, null,null);
            updateResult.Count.Should().Be(1);
            updateResult.First().Name.Should().Be(GetUpdateModel(updateResult.First().Id).First().Name);
            updateResult.First().Description.Should().Be(GetUpdateModel(updateResult.First().Id).First().Description);
            updateResult.First().CurrentUnit.Should().Be(GetUpdateModel(updateResult.First().Id).First().CurrentUnit);
            updateResult.First().Number.Should().Be(GetUpdateModel(updateResult.First().Id).First().Number);
            updateResult.First().Price.Should().Be(GetUpdateModel(updateResult.First().Id).First().Price);


            await _delete.DeleteAsync(GetDeleteModel(updateResult.First().Id));
            var deleteResult = await _query.FindByOptionsAsync(updateResult.First().Id, null,null);
            deleteResult.Count.Should().Be(0);
        }

        public List<ProductCommandModel> GetInsertModel() => new List<ProductCommandModel> {
                new ProductCommandModel
                {
                    Name = $"Test{_guid}",
                    Description = $"Test{_guid}",
                    Number = $"Test{_guid}",
                    Price = 100,
                    CurrentUnit = 100,
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                }
            };

        public List<ProductCommandModel> GetUpdateModel(int id) => new List<ProductCommandModel> {
                new ProductCommandModel
                {
                    Id = id,
                    Name = $"UpdateTest{_guid}",
                    Description = $"UpdateTest{_guid}",
                    Number = $"Test{_guid}",
                    Price = 120,
                    CurrentUnit = 120,
                    IsValid = true,
                    UpdatedOn = _now,
                }
            };

        public List<ProductCommandModel> GetDeleteModel(int id) => new List<ProductCommandModel> {
                 new ProductCommandModel
                {
                    Id = id,
                    UpdatedOn = _now,
                }
            };
    }
}
