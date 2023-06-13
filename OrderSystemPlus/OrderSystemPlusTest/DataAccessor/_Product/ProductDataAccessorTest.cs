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
    public class ProductDataAccessorTest
    {
        private IProductRepository _repository;
        private DateTime _now;

        public ProductDataAccessorTest()
        {
            _now = DateTime.Now;
            _repository = new ProductRepository();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _repository.InsertAsync(new List<ProductDto> { GetInsertModel() });
            var insertResult = await _repository.FindByOptionsAsync(null, GetInsertModel().Name, GetInsertModel().Number);
            insertResult.Count.Should().Be(1);
            insertResult.First().Name.Should().Be(GetInsertModel().Name);
            insertResult.First().Description.Should().Be(GetInsertModel().Description);
            insertResult.First().Number.Should().Be(GetInsertModel().Number);
            insertResult.First().Price.Should().Be(GetInsertModel().Price);

            await _repository.UpdateAsync(new List<ProductDto> { GetUpdateModel(insertResult.First().Id) });
            var updateResult = await _repository.FindByOptionsAsync(insertResult.First().Id, null, null);
            updateResult.Count.Should().Be(1);
            updateResult.First().Name.Should().Be(GetUpdateModel(updateResult.First().Id).Name);
            updateResult.First().Description.Should().Be(GetUpdateModel(updateResult.First().Id).Description);
            updateResult.First().Number.Should().Be(GetUpdateModel(updateResult.First().Id).Number);
            updateResult.First().Price.Should().Be(GetUpdateModel(updateResult.First().Id).Price);

            await _repository.DeleteAsync(new List<ProductDto> { GetDeleteModel(updateResult.First().Id) });
            var deleteResult = await _repository.FindByOptionsAsync(updateResult.First().Id, null, null);
            deleteResult.Count.Should().Be(0);
        }

        public ProductDto GetInsertModel() =>
                new ProductDto
                {
                    Name = $"Test1",
                    Description = $"Test1",
                    Number = $"Test1",
                    Price = 100,
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                };

        public ProductDto GetUpdateModel(int id) =>
                new ProductDto
                {
                    Id = id,
                    Name = "Test2",
                    Description = "Test2",
                    Number = "Test2",
                    Price = 120,
                    IsValid = true,
                    UpdatedOn = _now,
                };

        public ProductDto GetDeleteModel(int id) =>
                 new ProductDto
                 {
                     Id = id,
                     UpdatedOn = _now,
                 };
    }
}
