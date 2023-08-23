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
    public class ProductTypeDataAccessorTest
    {
        private IProductTypeRepository _repository;
        private DateTime _now;

        public ProductTypeDataAccessorTest()
        {
            _now = DateTime.Now;
            _repository = new ProductTypeRepository();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _repository.InsertAsync(new List<ProductTypeDto> { GetInsertModel() });
            var (_, insertResult) = await _repository.FindByOptionsAsync(null, GetInsertModel().Name);
            insertResult.Count.Should().Be(1);
            insertResult.First().Name.Should().Be(GetInsertModel().Name);
            insertResult.First().Description.Should().Be(GetInsertModel().Description);

            await _repository.UpdateAsync(new List<ProductTypeDto> { GetUpdateModel(insertResult.First().Id) });
            var (_, updateResult) = await _repository.FindByOptionsAsync(insertResult.First().Id);
            updateResult.Count.Should().Be(1);
            updateResult.First().Name.Should().Be(GetUpdateModel(updateResult.First().Id).Name);
            updateResult.First().Description.Should().Be(GetUpdateModel(updateResult.First().Id).Description);

            await _repository.DeleteAsync(new List<ProductTypeDto> { GetDeleteModel(updateResult.First().Id) });
            var (_, deleteResult) = await _repository.FindByOptionsAsync(updateResult.First().Id);
            deleteResult.Count.Should().Be(0);
        }

        public ProductTypeDto GetInsertModel() =>
                new ProductTypeDto
                {
                    Name = $"Test1",
                    Description = $"Test1",
                    IsValid = true,
                    CreatedOn = _now,
                    UpdatedOn = _now,
                };

        public ProductTypeDto GetUpdateModel(int id) =>
                new ProductTypeDto
                {
                    Id = id,
                    Name = "Test2",
                    Description = "Test2",
                    IsValid = true,
                    UpdatedOn = _now,
                };

        public ProductTypeDto GetDeleteModel(int id) =>
                 new ProductTypeDto
                 {
                     Id = id,
                     UpdatedOn = _now,
                 };
    }
}
