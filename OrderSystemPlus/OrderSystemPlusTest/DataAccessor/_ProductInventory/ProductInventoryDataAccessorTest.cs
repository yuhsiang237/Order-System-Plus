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
    public class ProductInventoryDataAccessorTest
    {
        private IProductInventoryRepository _repository;
        private DateTime _now;

        public ProductInventoryDataAccessorTest()
        {
            _now = DateTime.Now;
            _repository = new ProductInventoryRepository();
        }

        [Fact]
        public async Task RunAsync()
        {
            await _repository.InsertAsync(new List<ProductInventoryDto> { GetInsertModel() });
            var insertResult = await _repository.FindByOptionsAsync(GetInsertModel().ProductId);
            insertResult.Count.Should().Be(1);
            insertResult.First().ProductId.Should().Be(GetInsertModel().ProductId);
            insertResult.First().AdjustQuantity.Should().Be(GetInsertModel().AdjustQuantity);
            insertResult.First().PrevTotalQuantity.Should().Be(GetInsertModel().PrevTotalQuantity);
            insertResult.First().TotalQuantity.Should().Be(GetInsertModel().TotalQuantity);
            insertResult.First().AdjustProductInventoryType.Should().Be(GetInsertModel().AdjustProductInventoryType);
            insertResult.First().Remark.Should().Be(GetInsertModel().Remark);

            await _repository.DeleteAsync(new List<ProductInventoryDto> { GetDeleteModel(insertResult.First().Id) });
            var deleteResult = await _repository.FindByOptionsAsync(Id: insertResult.First().Id);
            deleteResult.Count.Should().Be(0);
        }

        public ProductInventoryDto GetInsertModel() =>
               new ProductInventoryDto
               {
                   ProductId = 999999999,
                   AdjustQuantity = 100,
                   PrevTotalQuantity = 0,
                   TotalQuantity = 100,
                   AdjustProductInventoryType = OrderSystemPlus.Enums.AdjustProductInventoryType.Force,
                   Remark = "測試",
                   CreatedOn = _now,
                   UpdatedOn = _now,
                   IsValid = true,
               };

        public ProductInventoryDto GetDeleteModel(int id) =>
                 new ProductInventoryDto
                 {
                     Id = id,
                     UpdatedOn = _now,
                 };
    }
}
